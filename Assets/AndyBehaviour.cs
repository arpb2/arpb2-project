using ARPB2;
using System.Collections.Generic;
using UnityEngine;

public class AndyBehaviour : MonoBehaviour
{
   
    private Animator AndyAnimation;
    private DetectedPlatform platform;


    public void Start()
    {
        AndyAnimation = GetComponent<Animator>();
    }


    public void PlaceAndy(DetectedPlatform platform)
    {
        //Instantiate(AndyPrefab, platform.StartPoint, Quaternion.identity);
        gameObject.transform.position = platform.StartPoint;    // If using prefabs this souldnt be necessary
        gameObject.SetActive(true);
        this.platform = platform;
    }

    public void MoveForward(float distance)
    {
        List<Vector3> polygon = new List<Vector3>();
        Vector3 finalPosition = gameObject.transform.position - gameObject.transform.forward * distance;
        platform.GetBoundaryPolygon(polygon);
        if (GeometryUtils.PolyContainsPoint(polygon, finalPosition))
        {
            if (AndyAnimation.isActiveAndEnabled)
            {
                AndyAnimation.SetBool("IsAdvancing", true);
            }
            else
            {
                gameObject.transform.position = finalPosition;
            }
        }
    }

    public void OnMoveForwardFinished(float distance)
    {
        gameObject.transform.position -= gameObject.transform.forward * distance;
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
