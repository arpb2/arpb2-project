using GoogleARCore;
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

    /*
     * Calculates the intersection between an infinite line and a segment
     * Returns null if no intersection exists
     * 
     * Source: https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line
     */ 
    public static Vector2? CalculateLineSegmentIntersection(Vector2 l1, Vector2 l2, Vector2 s1, Vector2 s2)
    {
        // (l1, l2) define the line, (s1, s2) define the segment

        float denominator = (l1.x - l2.x) * (s1.y - s2.y) - (l1.y - l2.y) * (s1.x - s2.x);
        if (denominator == 0)
        {
            // Lines are parallel
            return null;
        }

        // We will treat the segments as infinite lines, and find where they intersect
        float t = ((l1.x - s1.x) * (s1.y - s2.y) - (l1.y - s1.y) * (s1.x - s2.x)) / denominator;
        float u = - ((l1.x - l2.x) * (l1.y - s1.y) - (l1.y - l2.y) * (l1.x - s1.x)) / denominator;

        // And now we check whether said intersection is inside the given segment
        if (0 <= u && u <= 1)
        {
            Vector2 intersection = new Vector2(l1.x + t * (l2.x - l1.x), l1.y + t * (l2.y - l1.y));
            return intersection;
        }
        else
        {
            // Intersection is outside one or both segments
            return null;
        }
    }

}
