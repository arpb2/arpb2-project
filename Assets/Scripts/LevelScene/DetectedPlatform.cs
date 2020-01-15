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

        public bool EqualsPlane(DetectedPlane plane)
        {
            return plane.GetHashCode() == this.PlaneCode;
        }

    }

}