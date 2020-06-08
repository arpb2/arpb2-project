using Newtonsoft.Json;

public class Collectible
{
    [JsonProperty("position")]
    public Coordinate Coordinate { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }


    public static Collectible Load(string json)
    {
        return JsonConvert.DeserializeObject<Collectible>(json);
    }
}
