using CourtList.Models;

namespace CourtList.Services;

public interface IAuthService
{
    bool ValidateLogin(AuthModel authModel);
}