using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class LevelSpecificationRequester : ARPB2Requester
{
    public readonly static string LEVEL_PATH = "/levels/{0}";

    public static void Get(MonoBehaviour mono, int levelNo, Action<LevelSpecification> onSuccess, Action<string> onFailure = null)
    {
        mono.StartCoroutine(DoGet(levelNo, onSuccess, onFailure));
    }

    private static IEnumerator DoGet(int levelNo, Action<LevelSpecification> onSuccess, Action<string> onFailure)
    {
        Debug.Log(">>> LevelSpecificationRequester.Get");
        UnityWebRequest www = UnityWebRequest.Get(GetLevelSpecificationURL(levelNo));
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            onSuccess(LevelSpecification.Load(www.downloadHandler.text));
        }
        else if (onFailure != null)
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
