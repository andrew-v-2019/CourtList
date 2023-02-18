using CourtList.Models;
using CourtList.Services.Constants;
using CourtList.Services.Models;

namespace CourtList.Services;

public class RegionService : IRegionService
{
    private readonly IHtmlService _htmlService;
    private readonly ICourtService _courtService;
    private readonly IConfigurationService _configurationService;

    public RegionService(ICourtService courtService, IConfigurationService configurationService)
    {
        _htmlService = new HtmlService();
        _courtService = courtService;
        _configurationService = configurationService;
    }

    private Region CreateRegionModel(Option regionOption)
    {
        var regionId = Convert.ToInt32(regionOption.Value);
        Console.WriteLine($"Get data for {regionOption.Text}");
        var courtList = _courtService.GetByRegionId(regionId);
        var region = new Region(regionId, regionOption.Text, courtList);
        return region;
    }

    private IReadOnlyCollection<Region> CreateRegionList(IReadOnlyCollection<Option> regionOptionList)
    {
        var regionList = regionOptionList.Select(CreateRegionModel)
            .Select(r => r)
            .ToList();
        return regionList;
    }

    private IReadOnlyCollection<Option> ApplyPagination(IReadOnlyCollection<Option> regionOptionList,
        Pagination pagination)
    {
        if (pagination.IsPaginationEnabled())
        {
            regionOptionList = regionOptionList.Skip(pagination.GetOffset())
                .Take(pagination.PageLength)
                .Select(x => x)
                .ToList();
        }

        return regionOptionList;
    }

    public RegionListModel GetRegionList(int page)
    {
        var url = _configurationService.GetString(Field.RegionUrl);
        var selectorId = _configurationService.GetString(Field.RegionSelectorId);
        var regionOptionList =
            _htmlService.LoadOptionList(url, selectorId);

        var pageLen = _configurationService.GetInt(Field.PageLength);
        var pagination = new Pagination(page, pageLen, regionOptionList.Count);

        regionOptionList = ApplyPagination(regionOptionList, pagination);

        var regionList = CreateRegionList(regionOptionList);
        var viewModel = new RegionListModel(pagination, regionList);
        return viewModel;
    }
}