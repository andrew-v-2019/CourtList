using CourtList.Services.Models;

namespace CourtList.Services;

internal interface IHtmlService
{
    IReadOnlyCollection<Option> LoadOptionList(string url, string selectId);
    IReadOnlyCollection<Option> LoadOptionList(string url);
}