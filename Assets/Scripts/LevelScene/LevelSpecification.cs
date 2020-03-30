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


    public static LevelSpecification Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelSpecification>(json);
    }


    public List<PlatformRequirement> GeneratePlatformRequirements()
    {
        return new List<PlatformRequirement>(1)
        {
            new PlatformRequirement(Rows, Columns)
        };
    }

}
