using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Models;

public class Pagination
{
    public Pagination()
    {
    }

    public Pagination(int page, int pageLength, int totalCount)
    {
        PageCount = (int)Math.Ceiling((decimal)(totalCount / (decimal)pageLength));
        if (page > PageCount)
        {
            page = PageCount;
        }

        Page = page == 0 ? 1 : page;
        PageLength = pageLength;
        TotalCount = totalCount;
    }


    public bool IsPaginationEnabled()
    {
        return Page > 0;
    }

    public int GetOffset()
    {
        var offset = (Page - 1) * PageLength;
        return offset;
    }

    [JsonProperty("page")]
    public int Page { get; set; }
    
    [JsonProperty("pageLength")]
    public int PageLength { get; set; }
    
    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    [JsonProperty("pageCount")]
    public int PageCount { get; set; }
}