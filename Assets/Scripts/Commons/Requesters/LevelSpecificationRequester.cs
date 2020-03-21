using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelSpecificationRequester : ARPB2Requester
{ 

    public readonly static string LEVEL_PATH = "/levels/{0}";
    
    public static IEnumerator Get(int levelNo)
    {
        UnityWebRequest www = UnityWebRequest.Get(GetLevelSpecificationURL(levelNo));
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            Debug.Log("Level JSON: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("HTTP REQUEST ERROR: " + www.error);
        }

        //return LevelSpecification.Load(DEBUG_SPEC);
    }

    public static string GetLevelSpecificationURL(int levelId)
    {
        return string.Format(BASE_URL + LEVEL_PATH, levelId);
    }

}
