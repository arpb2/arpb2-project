using ARPB2;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterBehaviour : MonoBehaviour
{
   
    private Animator AvatarAnimation;


    public void Start()
    {
        AvatarAnimation = GetComponent<Animator>();
    }

    public void DebugMoveForward(float distance)
    {
        MoveForward(distance);
    }

    public MovementResult MoveForward(float distance)
    {
        Debug.Log(">>> MISSING IMPLEMENTATION");
        return MovementResult.Error;
    }

    public void OnMoveForwardFinished(float distance)
    {
        gameObject.transform.position -= gameObject.transform.forward * distance;
        AvatarAnimation.SetBool("IsAdvancing", false);
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
