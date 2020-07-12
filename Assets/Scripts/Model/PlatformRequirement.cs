using ARPB2;
using GoogleARCore;

public class PlatformRequirement
{

    private const float EXTRA_SPACE = 1.1f;

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
        bool fits = boxDimensions[0] > MinimumRows * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE &&
            boxDimensions[1] > MinimumColumns * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE;
        bool fitsRotated = boxDimensions[1] > MinimumRows * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE &&
            boxDimensions[0] > MinimumColumns * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE;
        return fits || fitsRotated;
    }

}
