using CourtList.Models;
using CourtList.Services.Constants;

namespace CourtList.Services;

public class AuthService : IAuthService
{
    private readonly IConfigurationService _configurationService;

    public AuthService(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public bool ValidateLogin(AuthModel authModel)
    {
        var login = _configurationService.GetString(Field.Login).ToLower();
        var password = _configurationService.GetString(Field.Password);

        if(!authModel.Login.Equals(login) || !authModel.Password.Equals(password))
        {
            throw new UnauthorizedAccessException(login);
        }

        return true;
    }
}