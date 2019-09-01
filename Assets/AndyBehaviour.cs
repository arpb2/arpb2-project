using ARPB2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndyBehaviour : MonoBehaviour
{
    
    public float StepLength;
    
    private Animator AndyAnimation;


    public void Start()
    {
        AndyAnimation = GetComponent<Animator>();
    }


    public void PlaceAndy(DetectedPlatform platform)
    {
        //Instantiate(AndyPrefab, platform.StartPoint, Quaternion.identity);
        gameObject.transform.position = platform.StartPoint;    // If using prefabs this souldnt be necessary
        gameObject.SetActive(true);
    }

    public void MoveForward()
    {
        if (AndyAnimation.isActiveAndEnabled)
        {
            AndyAnimation.SetBool("IsAdvancing", true);
        }
        else
        {
            gameObject.transform.position -= gameObject.transform.forward * StepLength;
        }
    }

    public void OnMoveForwardFinished()
    {
        gameObject.transform.position -= gameObject.transform.forward * StepLength;
        AndyAnimation.SetBool("IsAdvancing", false);
    }

    public void TurnRight(float degrees)
    {
        gameObject.transform.Rotate(0, degrees, 0);
    }

    public void TurnLeft(float degrees)
    {
        gameObject.transform.Rotate(0, -degrees, 0);
    }

}
