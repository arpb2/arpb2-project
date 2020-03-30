using ARPB2;
using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRequirement
{

    public readonly int MinimumRows, MinimumColumns;

    public DetectedPlatform Platform = null;


    public PlatformRequirement(int minRows, int minColumns)
    {
        MinimumRows = minRows;
        MinimumColumns = minColumns;
    }

    public bool IsSatisfied()
    {
        return Platform != null;
    }

    public bool IsMetBy(DetectedPlane plane)
    {
        float[] boxDimensions = DetectedPlaneHelper.CalculateBoxDimensions(plane);
        bool fits = boxDimensions[0] > MinimumRows * PlatformBoardBehaviour.SQUARE_LENGTH &&
            boxDimensions[1] > MinimumColumns * PlatformBoardBehaviour.SQUARE_LENGTH;
        bool fitsRotated = boxDimensions[1] > MinimumRows * PlatformBoardBehaviour.SQUARE_LENGTH &&
            boxDimensions[0] > MinimumColumns * PlatformBoardBehaviour.SQUARE_LENGTH;
        return fits || fitsRotated;
    }

}
