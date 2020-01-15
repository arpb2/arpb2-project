using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPB2
{

    public class DetectedPlatformHelper
    {

        /**
         *  Traces a line between the center of both platforms an returns the point where
         *  this lines intercepts the plat1 boundary, ignoring y-value (height) on both platforms
         */
        public static Vector3? FindTeleportPadPosition(DetectedPlatform plat1, DetectedPlatform plat2)
        {
            if (plat1.EqualsPlane(plat2))
            {
                throw new Exception("FindTransportPadPosition received same platforms as arguments");
            }

            Vector3 thisCenter = plat1.CenterPose.position, thatCenter = plat2.CenterPose.position;
            List<Vector3> vertices = new List<Vector3>();
            Vector3 vertex1, vertex2;
            plat1.GetBoundaryPolygon(vertices);
            for (int i = 0; i < vertices.Count; ++i)
            {
                vertex1 = vertices[i];
                vertex2 = vertices[i + 1 < vertices.Count ? i + 1 : 0];
                Vector2? intersection = GeometryUtils.CalculateSegmentsIntersection(
                    thisCenter.XZ(), thatCenter.XZ(), vertex1.XZ(), vertex2.XZ(), 0.1f
                );
                if (intersection != null)
                {
                    return new Vector3(intersection.Value.x, vertex1.y, intersection.Value.y);
                }
            }

            // This should never happen
            throw new Exception("FindTeleportPadPosition: No intersection found");
        }
    }

}