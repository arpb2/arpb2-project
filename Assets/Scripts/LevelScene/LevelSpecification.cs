using Newtonsoft.Json;
using System.Collections.Generic;


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

    private static string DEBUG_SPEC = @"
        {
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

    private static string DEBUG_SPEC_2 = @"
    {
        ""origin"": {
            ""position"": {
                ""x"": 2,
                ""y"": 3
            },
            ""orientation"": ""N""
        },
        ""destination"": {
            ""position"": {
                ""x"": 10,
                ""y"": 10
            }
        },
        ""minimal_dimension"": {
            ""rows"": 2,
            ""columns"": 3
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
                    ""x"": 7,
                    ""y"": 9
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
                    ""x"": 7,
                    ""y"": 4
                }
            }
        ]
    }
";

    public static LevelSpecification Load(string json)
    {
        LevelSpecification level = JsonConvert.DeserializeObject<LevelSpecification>(json);
        level.GeneratePlatformRequirements();
        return level;
    }

    public static LevelSpecification LoadDebug(int number = 1)
    {
        return Load(number == 1 ? DEBUG_SPEC : DEBUG_SPEC_2);
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
