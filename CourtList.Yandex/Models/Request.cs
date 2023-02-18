namespace CourtList.Yandex;

public class Request
{
    public string httpMethod { get; set; } = string.Empty;
    public string body { get; set; } = string.Empty;
    public Dictionary<string, string> queryStringParameters { get; set; } = new Dictionary<string, string>();
}