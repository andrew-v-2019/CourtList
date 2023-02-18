using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CourtList.Models;

public class Region
{
    public Region(int id, string name, IReadOnlyCollection<Court> courtList)
    {
        Subject = new Subject(id, name);
        CourtList = courtList;
    }
    
    public Region()
    {
      
    }

    [JsonProperty("subject")]
    public Subject Subject { get; set; } = new Subject();

    [JsonProperty("child_courts")] 
    public IReadOnlyCollection<Court> CourtList { get; set; } = new List<Court>();
}