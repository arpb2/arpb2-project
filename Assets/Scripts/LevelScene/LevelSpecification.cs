using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelSpecification
{

    [JsonProperty("origin")]
    public readonly LevelOrigin Origin;

    [JsonProperty("minimal_dimension.rows")]
    public readonly int Rows;

    [JsonProperty("minimal_dimension.columns")]
    public readonly int Columns;


    public static LevelSpecification Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelSpecification>(json);
    }

}
