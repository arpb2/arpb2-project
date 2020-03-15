using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector3 Position;


    public Coordinate(int x, int y, Vector3? pos = null)
    {
        X = x;
        Y = y;
        if (pos.HasValue)
            Position = pos.Value;
    }
}
