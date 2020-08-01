using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelSpecification
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("origin")]
    public LevelOrigin Origin { get; set; }

    [JsonProperty("destination")]
    public LevelOrigin Destination { get; set; }

    [JsonProperty("minimal_dimensions.rows")]
    public int Rows { get; set; }

    [JsonProperty("minimal_dimensions.columns")]
    public int Columns { get; set; }

    [JsonProperty("pads")]
    public List<Pad> Pads { get; set; }

    [JsonProperty("collectibles")]
    public List<Collectible> Collectibles { get; set; }

    [JsonIgnore]
    public List<PlatformRequirement> PlatformRequirements { private set; get; }

    public static string DEBUG_SPEC = @"
        {
            ""id"": 1,
            ""minimal_dimensions"": {
                ""rows"": 5,
                ""columns"": 5
            },
            ""origin"": {
                ""position"": {
                    ""x"": 1,
                    ""y"": 1,
                },
                ""orientation"": ""N""
            },
            ""destination"": {
                ""position"": {
                    ""x"": 3,
                    ""y"": 3
                }
            },
            ""collectibles"": [
                {
                    ""position"": {
                        ""x"": 1,
                        ""y"": 2
                    }, 
                    ""type"": ""coin""
                },
                {
                    ""position"": {
                        ""x"": 4,
                        ""y"": 3
                    },
                    ""type"": ""key""
                }
            ],
            ""pads"": null
        }";

    public static string DEBUG_SPEC_2 = @"
    {
        ""id"": 2,
        ""origin"": {
            ""position"": {
                ""x"": 0,
                ""y"": 0
            },
            ""orientation"": ""N""
        },
        ""destination"": {
            ""position"": {
                ""x"": 6,
                ""y"": 6
            }
        },
        ""minimal_dimensions"": {
            ""rows"": 7,
            ""columns"": 7
        },
        ""collectibles"": [
            {
                ""position"": {
                    ""x"": 4,
                    ""y"": 4
                },
                ""type"": ""coin""
            },
            {
                ""position"": {
                    ""x"": 2,
                    ""y"": 3
                },
                ""type"": ""key""
            }
        ],
        ""pads"": [
            {
                ""position"": {
                    ""x"": 2,
                    ""y"": 4
                }
            },
            {
                ""position"": {
                    ""x"": 5,
                    ""y"": 5
                }
            }
        ]
    }
";

    public static LevelSpecification Load(string json)
    {
        return JsonConvert.DeserializeObject<LevelSpecification>(json);
    }

    public static LevelSpecification LoadDebug(int number = 1)
    {
        return Load(number == 1 ? DEBUG_SPEC : DEBUG_SPEC_2);
    }

    public string ToJSON()
    {
        return JsonConvert.SerializeObject(this);
    }


    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        this.GeneratePlatformRequirements();
    }

    public void GeneratePlatformRequirements() 
    {
        this.PlatformRequirements = new List<PlatformRequirement>(1)
        {
            new PlatformRequirement(Rows, Columns)
        };
    }
}
