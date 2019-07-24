//-----------------------------------------------------------------------
// 
// Copied from GoogleARCore.Examples.Common.DetectedPlaneGenerator
//
//-----------------------------------------------------------------------

namespace ARPB2
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

    /// <summary>
    /// Manages the visualization of detected planes in the scene.
    /// </summary>
    public class DetectedPlatformGenerator : MonoBehaviour
    {
        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// Minimum area to start level
        /// </summary>
        public float MinArea;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to
            // visualize them.
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane. The
                // transform is set to the origin with an identity rotation since the mesh for our
                // prefab is updated in Unity World coordinates.
                GameObject planeObject =
                    Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                // Sadly, Session.GetTrackables does not recognize DetectedPlatform
                DetectedPlatform platform = new DetectedPlatform(m_NewPlanes[i]);
                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(platform);
            }

            //_CheckAreaRequirement();
        }

        private void _CheckAreaRequirement()
        {
            try
            {
                List<DetectedPlatform> platforms = new List<DetectedPlatform>();
                _GetAllPlatforms(platforms);
                foreach (DetectedPlatform platform in platforms)
                {
                    if (platform.CalculateArea() > MinArea)
                    {
                        //Debug.Log("<<<<<<<<<<<<<< FOUND >>>>>>>>>>>>>>>>");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Esploto todo");
            }
        }

        private void _GetAllPlatforms(List<DetectedPlatform> list)
        {
            List<DetectedPlane> planes = new List<DetectedPlane>();
            Session.GetTrackables<DetectedPlane>(planes, TrackableQueryFilter.All);
            foreach (DetectedPlane plane in planes)
                list.Add(new DetectedPlatform(plane));
        }
    }
}
