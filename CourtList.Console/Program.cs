using CourtList.Models;
using CourtList.Services;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = ServiceInjector.BuilServiceProvider();
var regionService = serviceProvider.GetRequiredService<IRegionService>();
var pageNumber = -1000;

var regionList = regionService?.GetRegionList(pageNumber);
foreach (var region in regionList?.RegionList ?? new List<Region>())
{
    Console.WriteLine($"{region.Subject.Id}. {region.Subject.Name}");
    foreach (var court in region.CourtList)
    {
        Console.WriteLine($"   - {court.Code}. {court.Name}");
    }
}

Console.ReadLine();