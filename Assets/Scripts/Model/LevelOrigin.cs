using Newtonsoft.Json;
using System.Runtime.Serialization;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelOrigin
{

    [JsonProperty("position")]
    public Coordinate Coordinate { get; set; }

    // [JsonProperty("orientation")]
    public Orientation Orientation { get; set; }


    [OnDeserialized()]
    internal void ShouldNotBeDoingThis(StreamingContext context)
    {
        this.Orientation = Orientation.N;
    }

}
