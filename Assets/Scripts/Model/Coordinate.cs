using Newtonsoft.Json;

[JsonConverter(typeof(JsonPathConverter))]
public class Coordinate
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }


    public static Coordinate operator +(Coordinate c1, Coordinate c2)
    {
        return new Coordinate(c1.X + c2.X, c1.Y + c2.Y);
    }

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
