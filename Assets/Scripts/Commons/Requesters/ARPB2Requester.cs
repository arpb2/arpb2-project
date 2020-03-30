using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPB2Requester 
{

    public readonly static string BASE_URL = "http://18.188.179.106:5555";

    protected static void onDefaultFailure(string error)
    {
        Debug.LogError("HTTP request failed: " + error);
    }

}
