using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Models;

public class RegionListModel
{
    public RegionListModel(Pagination pagination, IReadOnlyCollection<Region> regionList)
    {
        Pagination = pagination;
        RegionList = regionList;
    }

    public RegionListModel(int page)
    {
        Pagination = new Pagination(page, 0, 0);
        RegionList = new List<Region>();
    }

    public RegionListModel()
    {
    }

    [JsonProperty("pagination")]
    public Pagination Pagination { get; set; } = new Pagination();

    [JsonProperty("regionList")]
    public IReadOnlyCollection<Region> RegionList { get; set; } = new List<Region>();
}