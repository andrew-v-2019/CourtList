using System.Text;
using CourtList.Services.Constants;
using CourtList.Services.Models;
using HtmlAgilityPack;

namespace CourtList.Services
{
    internal class HtmlService : IHtmlService
    {
        private HtmlDocument LoadDocument(string url)
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var webGet = new HtmlWeb();
            webGet.OverrideEncoding = Encoding.GetEncoding(Config.CodePage);
            var doc = webGet.Load(url);
            return doc;
        }

        public IReadOnlyCollection<Option> LoadOptionList(string url, string selectId)
        {
            var htmlDocument = LoadDocument(url);
            var selectElement = htmlDocument.GetElementbyId(selectId);
            var optionList = GetOptionList(selectElement);
            return optionList;
        }

        public IReadOnlyCollection<Option> LoadOptionList(string url)
        {
            var htmlDocument = LoadDocument(url);
            var optionList = GetOptionList(htmlDocument.DocumentNode);
            return optionList;
        }

        private IReadOnlyCollection<Option> GetOptionList(HtmlNode optionNode)
        {
            var optionNodeList = optionNode.Descendants(Html.OptionTagName)
                .Where(x => !string.IsNullOrWhiteSpace(x.InnerText.Trim()))
                .Select(x => x)
                .ToList();
            var optionList = optionNodeList
                .Select(n => new Option(n.Attributes[Html.ValueAttr].Value, n.InnerText.ShrinkString()))
                .OrderBy(x => x.Text)
                .ToList();
            return optionList;
        }
    }
}