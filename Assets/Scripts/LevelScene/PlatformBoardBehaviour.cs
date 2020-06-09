
using ARPB2;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBoardBehaviour : MonoBehaviour
{
    public static readonly float SQUARE_LENGTH = 0.15f;

    public GameObject BoardSquarePrefab;

    private BoardSquareBehaviour[,] boardSquares;
    private int rowsCount, columnsCount;
    private Coordinate boardOrigin;


    public void Build(PlatformRequirement requirement)
    {
        GenerateCoordinates(requirement.Platform);
        LocateBoard(requirement);
    }

    /// <summary>
    /// Instantiates the given prefab on the given coordinate, only if its square is free
    /// <param name="coords">Coordinates suggested by the level requirements.
    /// Note that this must be converted to boards coordinates, since they may vary</param>
    /// </summary>
    public GameObject LocateElement(GameObject prefab, Coordinate levelCoords)
    {
        Coordinate coords = levelCoords + boardOrigin;
        BoardSquareBehaviour square = GetBoardSquare(coords);

        if (square == null || !square.IsFree()) return null;

        Vector3 coordsPosition = GetBoardSquare(coords).transform.position;
        GameObject elementObject = Instantiate(prefab, coordsPosition, Quaternion.identity, transform);

        ElementBehaviour element = elementObject.GetComponent<ElementBehaviour>();
        if (element)
        {
            element.Location = coords;
        }

        square.SetElement(element);

        return elementObject;
    }

    public MovementResult MoveElement(ElementBehaviour element, Coordinate dest)
    {
        BoardSquareBehaviour square = GetBoardSquare(dest);

        if (square == null || !square.IsFree())
        {
            Debug.Log(">>> Could not move element from " + element.Location.ToString() + " to " + dest.ToString());
            return MovementResult.Unaccomplished;
        }

        // Switch element's square
        BoardSquareBehaviour currentSquare = GetBoardSquare(element.Location);
        currentSquare.RemoveElement();
        square.SetElement(element);
        element.Location = dest;

        return MovementResult.Success;
    }


    private BoardSquareBehaviour GetBoardSquare(Coordinate coord)
    {
        return boardSquares[coord.X, coord.Y];
    }

    private void GenerateCoordinates(DetectedPlatform platform)
    {
        // Variables initialization
        Vector3[] boundingBox = DetectedPlaneHelper.CalculateBoundingBox(platform);
        Vector3 startPoint = boundingBox[0], finishPoint = boundingBox[1];
        float boxWidth = finishPoint.x - startPoint.x;
        float boxDepth = finishPoint.z - startPoint.z;

        rowsCount = (int)Mathf.Floor(boxWidth / SQUARE_LENGTH);
        columnsCount = (int)Mathf.Floor(boxDepth / SQUARE_LENGTH);
        boardSquares = new BoardSquareBehaviour[rowsCount, columnsCount];

        List<Vector3> platformPolygon = new List<Vector3>();
        platform.GetBoundaryPolygon(platformPolygon);

        float planeHeight = platformPolygon[0].y;

        // We iterate over the bounding box collecting the points that belong to the platform
        for (int colNum = 0; colNum < columnsCount; colNum++)
        {
            float zValue = startPoint.z + SQUARE_LENGTH * (colNum + 1);
            for (int rowNum = 0; rowNum < rowsCount; rowNum++)
            {
                float xValue = startPoint.x + SQUARE_LENGTH * (rowNum + 1);
                Vector3 point = new Vector3(xValue, planeHeight, zValue);
                if (GeometryUtils.PolyContainsPoint(platformPolygon, point))
                {
                    GameObject square = Instantiate(BoardSquarePrefab, transform);
                    square.transform.position = point;
                    boardSquares[rowNum, colNum] = square.GetComponent<BoardSquareBehaviour>();
                    boardSquares[rowNum, colNum].SetDebugText(String.Format("({0}; {1})", rowNum, colNum));
                }
            }
        }
    }

    private void LocateBoard(PlatformRequirement requirement)
    {
        // With the info taken from GetRowsRanges() we iterate 
        //  each row until we find where we can locate the board

        int consecutiveValidRows = 0;
        int rowNum = 0, start = 0, end = columnsCount;
        List<Tuple<int, int>> RowsRanges = GetRowsRanges();

        while (consecutiveValidRows < requirement.MinimumRows && rowNum < rowsCount)
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
            boardOrigin = new Coordinate(originRow, originCol);
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

        List<Tuple<int, int>> RowsRanges = new List<Tuple<int, int>>(rowsCount);
        int rowNum, start, end;

        for (rowNum = 0; rowNum < rowsCount; rowNum++)
        {
            start = 0;
            end = 0;

            // First we skip the first null squares
            while (boardSquares[rowNum, start] == null && start < columnsCount - 1)
                start++;

            // And now we count how many columns the row has
            end = start;
            while (boardSquares[rowNum, end] != null && end < columnsCount - 1)
                end++;

            RowsRanges.Add(new Tuple<int, int>(start, end));
        }

        return RowsRanges;
    }

}
