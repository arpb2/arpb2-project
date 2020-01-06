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
        public bool IsInitial;

        private int PlaneCode;


        internal DetectedPlatform(IntPtr nativeHandle, NativeSession nativeApi) : base(nativeHandle, nativeApi)
        {
        }

        public DetectedPlatform(DetectedPlane plane) : base(plane.m_TrackableNativeHandle, plane.m_NativeSession)
        {
            // FORCED FOR DEBUG
            StartPoint = plane.CenterPose.position;
            PlaneCode = plane.GetHashCode();
        }

        public Vector3? FindTransportPadPosition(DetectedPlatform that)
        {
            if (this.PlaneCode == that.PlaneCode)
            {
                throw new Exception("FoundTransportPadPosition received 'this' as argument");
            }

            Vector3 thisCenter = this.CenterPose.position, thatCenter = that.CenterPose.position;
            List<Vector3> vertices = new List<Vector3>();
            Vector3 vertex1, vertex2;
            GetBoundaryPolygon(vertices);
            for (int i = 0; i < vertices.Count; ++i)
            {
                vertex1 = vertices[i];
                vertex2 = vertices[i + 1 < vertices.Count ? i + 1 : 0];
                Vector2? intersection = GeometryUtils.CalculateSegmentsIntersection(
                    vertex1.XZ(), vertex2.XZ(), thisCenter.XZ(), thatCenter.XZ()
                );
                if (intersection != null)
                {
                    return new Vector3(intersection.Value.x, vertex1.y, intersection.Value.y);
                }
            }
            return null;
        }

        public bool EqualsPlane(DetectedPlane plane)
        {
            return plane.GetHashCode() == this.PlaneCode;
        }

    }

}