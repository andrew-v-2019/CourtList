﻿using CourtList.Services;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = ServiceInjector.BuilServiceProvider();

var regionService = serviceProvider.GetService<IRegionService>();
if (regionService == null)
{
    return;
}

var regionList = regionService?.GetRegionList(-1);

if (regionList?.RegionList == null)
{
    return;
}

foreach (var region in regionList.RegionList)
{
    Console.WriteLine($"{region.Subject.Id}. {region.Subject.Name}");
    foreach (var court in region.CourtList)
    {
        Console.WriteLine($"   - {court.Code}. {court.Name}");
    }
}