using Newtonsoft.Json;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelDestination
{

    [JsonProperty("position")]
    public Coordinate Coordinate { get; set; }

    [JsonProperty("orientation")]
    public Orientation Orientation { get; set; }


    public static LevelDestination Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelDestination>(json);
    }

}
