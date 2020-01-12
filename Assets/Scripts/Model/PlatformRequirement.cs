using ARPB2;
using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRequirement
{

    public readonly float MinimumArea;
    public readonly float? Height;
    public readonly bool IsInitial = false;

    public DetectedPlatform Platform = null;


    public PlatformRequirement(float minArea, float? height, bool isInitial = false)
    {
        this.MinimumArea = minArea;
        this.Height = height;
        this.IsInitial = isInitial;
    }

    public bool IsSatisfied()
    {
        return Platform != null;
    }

    public bool IsMetBy(DetectedPlane plane)
    {
        return GeometryUtils.CalculatePlaneArea(plane) >= MinimumArea;
    }

}
