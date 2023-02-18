using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Models;

public class Court
{
    public Court(string code, string name)
    {
        Code = code;
        Name = name;
    }
    
    public Court()
    {
       
    }

    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}