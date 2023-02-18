using CourtList.Models;

namespace CourtList.Services;

public interface IRegionService
{
    RegionListModel GetRegionList(int page);
}