using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Yandex;

public class ErrorBody
{
    public ErrorBody(string message)
    {
        Error = message;
    }

    [JsonProperty("error")]
    public string Error { get; set; }
}