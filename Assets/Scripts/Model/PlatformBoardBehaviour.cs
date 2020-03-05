
using ARPB2;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBoardBehaviour : MonoBehaviour
{
    public static readonly float SQUARE_LENGTH = 0.15f;

    private List<Vector3> SquaresPosition;


    public void Build(DetectedPlatform platform)
    {
        Vector3[] boundingBox = DetectedPlaneHelper.CalculateBoundingBox(platform);
        Vector3 startPoint = boundingBox[0], finishPoint = boundingBox[1];
        List<Vector3> platformPolygon = new List<Vector3>();
        platform.GetBoundaryPolygon(platformPolygon);
        float planeHeight = platformPolygon[0].y;
        SquaresPosition = new List<Vector3>();

        // We iterate over the bounding box collecting the points that belong to the platform
        for (float z = startPoint.z + SQUARE_LENGTH; z < finishPoint.z;  z += SQUARE_LENGTH)
        {
            for (float x = startPoint.x + SQUARE_LENGTH; x < finishPoint.x; x += SQUARE_LENGTH)
            {
                Vector3 point = new Vector3(x, planeHeight, z);
                if (GeometryUtils.PolyContainsPoint(platformPolygon, point))
                {
                    Debug.Log(string.Format("Point {0} is inside polygon", point));
                    SquaresPosition.Add(point);
                    GameObject debugSphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                    debugSphere.transform.position = point;
                    debugSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
                else
                {
                    Debug.Log(string.Format("Point {0} is outside polygon", point));
                }
            }
        }

        Utils.ShowAndroidToastMessage(string.Format("Board build with {0} points", SquaresPosition.Count));
    }

}
