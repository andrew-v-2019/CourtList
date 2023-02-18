namespace CourtList.Services;

public interface IConfigurationService
{
    int GetInt(string configurationKey);
    string GetString(string configurationKey);
}