using Newtonsoft.Json;

[JsonConverter(typeof(JsonPathConverter))]
public class Coordinate
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinate()
    {
        // Needed for JsonConverter
    }

    override public string ToString()
    {
        return string.Format("({0}; {1})", X, Y);
    }
}
