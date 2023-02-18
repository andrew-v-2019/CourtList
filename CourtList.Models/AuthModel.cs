namespace CourtList.Models;

public class AuthModel
{
    public AuthModel(string login, string pass)
    {
        Login = login;
        Password = pass;
    }

    public string Login { get; set; }
    public string Password { get; set; }
}