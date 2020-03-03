using Newtonsoft.Json;


public class LevelOrigin
{

    public Position Position { get; set; }
    public Orientation Orientation { get; set; }


    public static LevelOrigin Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelOrigin>(json);
    }

}
