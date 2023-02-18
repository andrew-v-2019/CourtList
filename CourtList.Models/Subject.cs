using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Models;

public class Subject
{
    public Subject(int id, string name)
    {
        Name = name;
        Id = id;
    }
    
    public Subject()
    {
    }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("id")] 
    public int Id { get; set; } = 0;
}