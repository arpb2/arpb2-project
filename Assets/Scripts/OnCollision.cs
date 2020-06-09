using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("stay");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("exit");
    }
}
