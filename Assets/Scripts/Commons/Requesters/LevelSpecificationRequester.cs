using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Runtime.Serialization;


[JsonConverter(typeof(JsonPathConverter))]
class LevelResponse {

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("definition")]
    public LevelSpecification Level { get; set; }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context) {
        this.Level.GeneratePlatformRequirements();        
    }

}

public class LevelSpecificationRequester : ARPB2Requester
{
    public readonly static string LEVEL_PATH = "/levels/{0}";

    public static void Get(MonoBehaviour mono, int levelNo, Action<LevelSpecification> onSuccess, Action<string> onFailure = null)
    {
        Debug.Log("load level no " + levelNo);
        mono.StartCoroutine(DoGet(levelNo, onSuccess, onFailure));
    }

    private static IEnumerator DoGet(int levelNo, Action<LevelSpecification> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Get(GetLevelSpecificationURL(levelNo));
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            Debug.Log(">>> LevelSpecificationRequester.Get, response: " + www.downloadHandler.text);
            LevelResponse response = JsonConvert.DeserializeObject<LevelResponse>(www.downloadHandler.text);

            if (response.Level != null) 
            {
                response.Level.Id = response.Id;
                response.Level.GeneratePlatformRequirements();
                onSuccess(response.Level);
            }
            else if (onFailure != null)
            {
                onFailure(string.Format("Level {0} has no field 'definition'", levelNo));
            }
        }

        if (onFailure != null)
        {
            onFailure(www.error);
        }
        else
        {
            onDefaultFailure(www.error);
        }
    }

    public static string GetLevelSpecificationURL(int levelId)
    {
        return string.Format(BASE_URL + LEVEL_PATH, levelId);
    }

}
