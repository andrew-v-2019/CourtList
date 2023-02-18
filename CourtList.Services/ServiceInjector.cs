using Microsoft.Extensions.DependencyInjection;

namespace CourtList.Services;

public static class ServiceInjector
{
    public static IServiceProvider BuilServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IRegionService, RegionService>()
            .AddScoped<ICourtService, CourtService>()
            .AddScoped<IAuthService, AuthService>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .BuildServiceProvider();

        return serviceProvider;
    }
}