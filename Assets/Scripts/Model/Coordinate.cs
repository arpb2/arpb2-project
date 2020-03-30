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

    override public string ToString()
    {
        return string.Format("({0}; {1}) at position {2}", X, Y, Position.ToString());
    }
}
