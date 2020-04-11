
using ARPB2;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBoardBehaviour : MonoBehaviour
{
    public static readonly float SQUARE_LENGTH = 0.15f;

    public GameObject BoardSquarePrefab;
    public MainCharacterBehaviour MainCharacter;

    private BoardSquareBehaviour[,] BoardSquares;
    private int RowsCount, ColumnsCount;
    private Coordinate BoardOrigin;


    public void Build(PlatformRequirement requirement)
    {
        GenerateCoordinates(requirement.Platform);
        LocateBoard(requirement);
        Debug.Log(">>> Finished building board");
    }

    public void LocateElements(LevelSpecification level)
    {
        Vector3 originPosition = GetBoardSquare(level.Origin.Coordinate).transform.position;
        Debug.Log(">>> Locating main char at " + originPosition.ToString());
        MainCharacter.transform.position = originPosition;
    }


    private BoardSquareBehaviour GetBoardSquare(Coordinate coord)
    {
        return BoardSquares[coord.X, coord.Y];
    }

    private void GenerateCoordinates(DetectedPlatform platform)
    {
        // Variables initialization
        Vector3[] boundingBox = DetectedPlaneHelper.CalculateBoundingBox(platform);
        Vector3 startPoint = boundingBox[0], finishPoint = boundingBox[1];
        float boxWidth = finishPoint.x - startPoint.x;
        float boxDepth = finishPoint.z - startPoint.z;

        RowsCount = (int)Mathf.Floor(boxWidth / SQUARE_LENGTH);
        ColumnsCount = (int)Mathf.Floor(boxDepth / SQUARE_LENGTH);
        BoardSquares = new BoardSquareBehaviour[RowsCount, ColumnsCount];

        List<Vector3> platformPolygon = new List<Vector3>();
        platform.GetBoundaryPolygon(platformPolygon);

        float planeHeight = platformPolygon[0].y;

        // We iterate over the bounding box collecting the points that belong to the platform
        for (int colNum = 0; colNum < ColumnsCount; colNum++)
        {
            float zValue = startPoint.z + SQUARE_LENGTH * (colNum + 1);
            for (int rowNum = 0; rowNum < RowsCount; rowNum++)
            {
                float xValue = startPoint.x + SQUARE_LENGTH * (rowNum + 1);
                Vector3 point = new Vector3(xValue, planeHeight, zValue);
                if (GeometryUtils.PolyContainsPoint(platformPolygon, point))
                {
                    GameObject square = Instantiate(BoardSquarePrefab, transform);
                    square.transform.position = point;
                    BoardSquares[rowNum, colNum] = square.GetComponent<BoardSquareBehaviour>();
                }
            }
        }
    }

    private void LocateBoard(PlatformRequirement requirement)
    {
        // With the info taken from _GetRowsRanges() we iterate 
        //  each row until we find where we can locate the board

        int consecutiveValidRows = 0;
        int rowNum = 0, start = 0, end = ColumnsCount;
        List<Tuple<int, int>> RowsRanges = GetRowsRanges();

        while (consecutiveValidRows < requirement.MinimumRows && rowNum < RowsCount)
        {
            start = Math.Max(start, RowsRanges[rowNum].Item1);
            end = Math.Min(end, RowsRanges[rowNum].Item2);

            if (end - start + 1 >= requirement.MinimumColumns)
                consecutiveValidRows++;
            else
                consecutiveValidRows = 0;

            rowNum++;
        }

        bool locationFound = consecutiveValidRows >= requirement.MinimumRows;
        if (locationFound)
        {
            int originRow = rowNum - consecutiveValidRows;
            int originCol = start;
            BoardOrigin = new Coordinate(originRow, originCol);
        }
        else
        {
            Utils.ShowAndroidToastMessage("No se pudo insertar el tablero");
            throw new Exception("Board locations failed");
        }
    }

    private List<Tuple<int, int>> GetRowsRanges()
    {
        // For each row we return at which column it starts and ends 

        List<Tuple<int, int>> RowsRanges = new List<Tuple<int, int>>(RowsCount);
        int rowNum, start, end;

        for (rowNum = 0; rowNum < RowsCount; rowNum++)
        {
            start = 0;
            end = 0;

            // First we skip the first null squares
            while (BoardSquares[rowNum, start] == null && start < ColumnsCount - 1)
                start++;

            // And now we count how many columns the row has
            end = start;
            while (BoardSquares[rowNum, end] != null && end < ColumnsCount - 1)
                end++;

            RowsRanges.Add(new Tuple<int, int>(start, end));
        }

        return RowsRanges;
    }

}
