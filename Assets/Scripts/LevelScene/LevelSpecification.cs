using Newtonsoft.Json;
using System.Collections.Generic;


[JsonConverter(typeof(JsonPathConverter))]
public class LevelSpecification
{

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
        "    }," +
        " \"collectibles\": [" +
        "    {" +
        "        \"position\": {" +
        "            \"x\": 1," +
        "            \"y\": 2" +
        "        }, " +
        "        \"type\": \"coin\"" +
        "    }," +
        "    {" +
        "        \"position\": {" +
        "            \"x\": 4," +
        "            \"y\": 3" +
        "        }," +
        "        \"type\": \"key\"" +
        "    }" +
        "]" +
        "  }" +
        "}";

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

    public static LevelSpecification LoadDebug()
    {
        return Load(DEBUG_SPEC_2);
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
