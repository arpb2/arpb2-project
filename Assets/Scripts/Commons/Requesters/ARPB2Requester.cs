using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPB2Requester 
{

    public readonly static string BASE_URL = "http://3.16.213.100:5555";

    protected static void onDefaultFailure(string error)
    {
        Debug.LogError("HTTP request failed: " + error);
    }

}
