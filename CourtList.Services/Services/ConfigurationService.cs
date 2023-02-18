using CourtList.Services.Constants;
using Microsoft.Extensions.Configuration;

namespace CourtList.Services;

public class ConfigurationService : IConfigurationService
{
    private IConfiguration _configRoot;

    public ConfigurationService()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), Config.ConfigFile);
        configurationBuilder.AddJsonFile(path, false);
        var root = configurationBuilder.Build();
        _configRoot = root;
    }

    private T Get<T>(string configurationKey)
    {
        var val = _configRoot?.GetSection(configurationKey)?.Value ?? string.Empty;
        T result = (T)Convert.ChangeType(val, typeof(T));
        return result;
    }

    public int GetInt(string configurationKey)
    {
        return Get<int>(configurationKey);
    }

    public string GetString(string configurationKey)
    {
        return Get<string>(configurationKey).Trim();
    }
}