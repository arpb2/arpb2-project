﻿using GoogleARCore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GeometryUtils
{
  
    /**
     * Checks if point is inside of any triangle formed by a polygon side and the center
    */
    public static bool PolyContainsPoint(List<Vector3> delimeter, Vector3 point)
    {
        int grade = delimeter.Count;
        Vector3 center = CalculateCenter(delimeter);
        for (int i = 0; i < grade; ++i)
        {
            // Check if points is inside triangle using this method: https://math.stackexchange.com/questions/51326
            Vector3 a = delimeter[i];
            Vector3 b = delimeter[i + 1 >= grade ? 0 : i + 1] - a;
            Vector3 c = center - a;
            Vector3 p = point - a;
            float d = b.x * c.z - c.x * b.z;
            float wa = (p.x * (b.z - c.z) + p.z * (c.x - b.x) + b.x * c.z - c.x * b.z ) / d;
            float wb = (p.x * c.z - p.z * c.x) / d;
            float wc = (p.z * b.x - p.x * b.z) / d;

            if (wa >= 0 && wa <= 1 && wb >= 0 && wb <= 1 && wc >= 0 && wc <= 1)
                return true;
        }
        return false;
    }

    public static Vector3 CalculateCenter(List<Vector3> points)
    {
        Vector3 center = new Vector3(0, 0, 0);
        for (int i = 0; i < points.Count; ++i)
        {
            center += points[i];
        }
        center /= points.Count;
        return center;
    }

}
