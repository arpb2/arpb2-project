using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelSpecification
{

    [JsonProperty("origin")]
    public LevelOrigin Origin { get; set; }

    [JsonProperty("minimal_dimensions.rows")]
    public int Rows { get; set; }

    [JsonProperty("minimal_dimensions.columns")]
    public int Columns { get; set; }

    [JsonIgnore]
    public List<PlatformRequirement> PlatformRequirements { private set; get; }

    private static string DEBUG_SPEC = "" +
        "{" +
        "  \"minimal_dimensions\": {" +
        "    \"rows\": 2," +
        "    \"columns\": 3" +
        "  }," +
        "  \"origin\": {" +
        "    \"position\": {" +
        "      \"x\": 2," +
        "      \"y\": 2," +
        "    }" +
        "  }" +
        "}";

    public static LevelSpecification Load(string json)
    {
        LevelSpecification level = JsonConvert.DeserializeObject<LevelSpecification>(json);
        level.GeneratePlatformRequirements();
        return level;
    }

    public static LevelSpecification LoadDebug()
    {
        return Load(DEBUG_SPEC);
    }

    public string ToJSON()
    {
        return JsonConvert.SerializeObject(this);
    }


    private List<PlatformRequirement> GeneratePlatformRequirements()
    {
        this.PlatformRequirements = new List<PlatformRequirement>(1)
        {
            new PlatformRequirement(Rows, Columns)
        };

        return this.PlatformRequirements;
    }
}
