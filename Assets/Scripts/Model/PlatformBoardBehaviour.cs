
using ARPB2;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBoardBehaviour : MonoBehaviour
{
    public static readonly float SQUARE_LENGTH = 0.15f;

    private Coordinate[,] Coordinates;
    private int RowsCount, ColumnsCount;
    private Coordinate BoardOrigin;


    public void Build(PlatformRequirement requirement)
    {
        _GenerateCoordinates(requirement.Platform);
        _LocateBoard(requirement);
        
        Utils.ShowAndroidToastMessage(string.Format("Board built"));
    }


    private void _GenerateCoordinates(DetectedPlatform platform)
    {
        Vector3[] boundingBox = DetectedPlaneHelper.CalculateBoundingBox(platform);
        Vector3 startPoint = boundingBox[0], finishPoint = boundingBox[1];
        float boxWidth = finishPoint.x - startPoint.x;
        float boxDepth = finishPoint.z - startPoint.z;

        RowsCount = (int)Mathf.Floor(boxWidth / SQUARE_LENGTH);
        ColumnsCount = (int)Mathf.Floor(boxDepth / SQUARE_LENGTH);
        Coordinates = new Coordinate[RowsCount, ColumnsCount];

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
                    Coordinates[rowNum, colNum] = new Coordinate(rowNum, colNum, point);
                    GameObject debugSphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                    debugSphere.transform.position = point;
                    debugSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
            }
        }
    }

    private void _LocateBoard(PlatformRequirement requirement)
    {
        // For each row we see at what column the board starts and ends
        // With that info we can know where to fit the board

        List<Tuple<int,int>> ColumnFits = new List<Tuple<int,int>>(ColumnsCount);
        int rowNum, start, end;
        for (rowNum = 0; rowNum < RowsCount; rowNum++)
        {
            start = 0;
            end = 0;

            // First we skip the first null squares
            while (Coordinates[rowNum, start] == null && start < ColumnsCount - 1)
                start++;

            // And now we count how many columns the row has
            end = start;
            while (Coordinates[rowNum, end] != null && end < ColumnsCount - 1)
                end++;

            ColumnFits.Add(new Tuple<int, int>(start, end));
        }

        // Now we search a valid location
        int consecutiveValidRows = 0;
        rowNum = 0;
        start = 0;
        end = ColumnsCount;
        while (consecutiveValidRows < requirement.MinimumRows && rowNum < RowsCount)
        {
            start = Math.Max(start, ColumnFits[rowNum].Item1);
            end = Math.Min(end, ColumnFits[rowNum].Item2);

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
            GameObject debugSphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            debugSphere.transform.position = Coordinates[originRow, originCol].Position;
            debugSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GameObject debugSphere2 = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            debugSphere2.transform.position = Coordinates[originRow + requirement.MinimumRows, originCol + requirement.MinimumColumns].Position;
            debugSphere2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else
        {
            Utils.ShowAndroidToastMessage("Se pudrió todo");
            throw new Exception("Se pudrió todo");
        }
    }
}
