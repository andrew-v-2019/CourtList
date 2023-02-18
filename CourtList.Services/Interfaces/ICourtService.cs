using CourtList.Models;

namespace CourtList.Services;

public interface ICourtService
{
    IReadOnlyCollection<Court> GetByRegionId(int regionId);
}