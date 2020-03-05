using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedPlaneHelper
{

    /* We will assume the polygon is strictly convex, so we sum up the area
     * of the triangles formed by each side and the center of the polygon 
     */
    public static float CalculateArea(DetectedPlane plane)
    {
        float area = 0f;
        Vector3 p1, p2;
        List<Vector3> polygon = new List<Vector3>();
        plane.GetBoundaryPolygon(polygon);

        Vector3 c = plane.CenterPose.position;
        for (int i = 0; i < polygon.Count; ++i)
        {
            p1 = polygon[i];
            p2 = polygon[i + 1 < polygon.Count ? i + 1 : 0];
            area += Math.Abs(p1.x * (p2.z - c.z) + p2.x * (c.z - p1.z) + c.x * (p1.z - p2.z)) / 2;
        }

        return area;
    }

    /*
     * Returns the bounding box dimensions in the format of (width, height)
     */
    public static float[] CalculateBoxDimensions(DetectedPlane plane)
    {
        Vector3[] box = CalculateBoundingBox(plane);
        float minX = box[0].x, minZ = box[0].z;
        float maxX = box[1].x, maxZ = box[1].z;

        return new float[] { maxX - minX, maxZ - minZ };
    }

    /* 
     * Returns two points of the plane's bounding box in the format of
     * (minX, 0, minZ) and (maxX, 0, maxZ)
     */
    public static Vector3[] CalculateBoundingBox(DetectedPlane plane)
    {
        List<Vector3> polygon = new List<Vector3>();
        plane.GetBoundaryPolygon(polygon);
        float minX = Mathf.Infinity, maxX = -Mathf.Infinity,
            minZ = Mathf.Infinity, maxZ = -Mathf.Infinity;

        foreach (Vector3 point in polygon)
        {
            minX = Mathf.Min(point.x, minX);
            maxX = Mathf.Max(point.x, maxX);
            minZ = Mathf.Min(point.z, minZ);
            maxZ = Mathf.Max(point.z, maxZ);
        }

        return new Vector3[]
        {
            new Vector3(minX, 0, minZ),
            new Vector3(maxX, 0, maxZ)
        };
    }

}
