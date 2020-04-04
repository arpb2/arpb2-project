using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenBehaviour : MonoBehaviour
{

    private Action OnLevelLoadedCallback = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetOnLevelLoadedCallback(Action callback)
    {
        this.OnLevelLoadedCallback = callback;
    }
}
