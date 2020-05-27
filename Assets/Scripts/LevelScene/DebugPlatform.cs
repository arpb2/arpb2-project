using UnityEngine;
using ARPB2;
using System.Collections.Generic;

class DebugPlatform : DetectedPlatform
{

    public DebugPlatform() : base(new System.IntPtr(), null) { }

    override public void GetBoundaryPolygon(List<Vector3> points)
    {
        Debug.Log(">>> Mocked GetBoundaryPolygon");
        points.Clear();
        points.AddRange(new Vector3[]{
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(1, 0, -1)
            });
    }
}