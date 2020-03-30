using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Components that holds the BoardSquare logic 
 */

public class BoardSquareBehaviour : MonoBehaviour
{

    Coordinate Coordinate;


    public void Initialize(Coordinate coordinate)
    {
        this.Coordinate = coordinate;
        _InstantiateAt(coordinate.Position);
    }


    private void _InstantiateAt(Vector3 position)
    {
        GameObject debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        debugSphere.transform.parent = this.transform;
        debugSphere.transform.position = position;
        debugSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }
}
