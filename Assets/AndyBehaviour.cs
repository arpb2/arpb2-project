using ARPB2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndyBehaviour : MonoBehaviour
{

    public GameObject AndyPrefab;

    private GameObject AndyObject;
    private Animator AndyAnimation;


    public void Start()
    {
        AndyAnimation = GetComponent<Animator>();
    }


    public void PlaceAndy(DetectedPlatform platform)
    {
        AndyObject = Instantiate(AndyPrefab, platform.StartPoint, Quaternion.identity);
    }

    public void MoveForward(float distance)
    {
        AndyAnimation.SetBool("IsMoving", true);
        AndyObject.transform.position += AndyObject.transform.forward * distance;
        AndyAnimation.SetBool("IsMoving", false);
    }

    public void TurnRight(float degrees)
    {
        AndyObject.transform.Rotate(0, degrees, 0);
    }

    public void TurnLeft(float degrees)
    {
        AndyObject.transform.Rotate(0, -degrees, 0);
    }

}
