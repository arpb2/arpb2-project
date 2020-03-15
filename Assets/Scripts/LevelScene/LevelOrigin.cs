using Newtonsoft.Json;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelOrigin
{

    [JsonProperty("position")]
    public Coordinate Position { get; set; }

    [JsonProperty("orientation")]
    public Orientation Orientation { get; set; }


    public static LevelOrigin Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelOrigin>(json);
    }

}
