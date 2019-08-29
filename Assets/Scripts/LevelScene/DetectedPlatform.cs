// ----------------------------------------------------------------------------
//
// Class derived from DetectedPlane
//
// ----------------------------------------------------------------------------

namespace ARPB2
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using GoogleARCore;
    using GoogleARCoreInternal;

    public class DetectedPlatform : GoogleARCore.DetectedPlane
    {

        public Vector3 StartPoint { get; set; }


        internal DetectedPlatform(IntPtr nativeHandle, NativeSession nativeApi) : base(nativeHandle, nativeApi)
        {
        }

        public DetectedPlatform(DetectedPlane plane) : base(plane.m_TrackableNativeHandle, plane.m_NativeSession)
        {
            // FORCED FOR DEBUG
            StartPoint = plane.CenterPose.position;
        }

        /// <summary>
        /// We will assume that the center of the polygon is inside it
        /// </summary>
        public float CalculateArea()
        {
            float area = 0f;
            Vector3 p1, p2;
            List<Vector3> polygon = new List<Vector3>();
            GetBoundaryPolygon(polygon);

            Vector3 c = CenterPose.position;
            for (int i = 0; i < polygon.Count; ++i)
            {
                p1 = polygon[i];
                p2 = polygon[i + 1 < polygon.Count ? i + 1 : 0];
                area += Math.Abs(p1.x * (p2.z - c.z) + p2.x * (c.z - p1.z) + c.x * (p1.z - p2.z)) / 2;
            }

            return area;
        }
    }

}