using ARPB2;
using GoogleARCore;

public class PlatformRequirement
{

    private const float EXTRA_SPACE = 1.2f;

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

    /*
    * This method checks whether a given plane meets the Platform Requirement.
    * It could be improved a lot, current implementation is more like a workaround ¯\_(ツ)_/¯
    */
    public bool IsMetBy(DetectedPlane plane)
    {
        float[] boxDimensions = DetectedPlaneHelper.CalculateBoxDimensions(plane);
        bool fits = boxDimensions[0] > 8 * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE &&
            boxDimensions[1] > 8 * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE;
        bool fitsRotated = boxDimensions[1] > 8 * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE &&
            boxDimensions[0] > 8 * PlatformBoardBehaviour.SQUARE_LENGTH * EXTRA_SPACE;
        return fits || fitsRotated;
    }

}
