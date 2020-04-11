// ----------------------------------------------------------------------------
//
// Class derived from DetectedPlane, used sort of as a wrapper
//
// ----------------------------------------------------------------------------

namespace ARPB2
{
    using System;
    using UnityEngine;
    using GoogleARCore;
    using GoogleARCoreInternal;

    public class DetectedPlatform : GoogleARCore.DetectedPlane
    { 
        private int PlaneCode;

        internal DetectedPlatform(IntPtr nativeHandle, NativeSession nativeApi) : base(nativeHandle, nativeApi)
        {
        }

        public DetectedPlatform(DetectedPlane plane) : base(plane.m_TrackableNativeHandle, plane.m_NativeSession)
        {
            PlaneCode = plane.GetHashCode();
        }


        public bool EqualsPlane(DetectedPlane plane)
        {
            return plane.GetHashCode() == this.PlaneCode;
        }

    }

}