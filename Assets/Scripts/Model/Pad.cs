using Newtonsoft.Json;

public class Pad
{
    [JsonProperty("position")]
    public Coordinate Coordinate { get; set; }


    public static Pad Load(string json)
    {
        return JsonConvert.DeserializeObject<Pad>(json);
    }
}
