using CourtList.Models;
using CourtList.Services.Constants;

namespace CourtList.Services;

public class CourtService : ICourtService
{
    private readonly IHtmlService _htmlService;
    private readonly IConfigurationService _configurationService;

    public CourtService(IConfigurationService configurationService)
    {
        _htmlService = new HtmlService();
        _configurationService = configurationService;
    }

    public IReadOnlyCollection<Court> GetByRegionId(int regionId)
    {
        var url = _configurationService.GetString(Field.CourtUrlTemplate);
        var courtListUrl = string.Format(url, regionId);
        var courtOptionList = _htmlService.LoadOptionList(courtListUrl);
        var courtList = courtOptionList.Select(option => new Court(option.Value, option.Text))
            .ToList();
        return courtList;
    }
}