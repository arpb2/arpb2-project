//-----------------------------------------------------------------------
// 
// Copied from GoogleARCore.Examples.Common.DetectedPlaneGenerator
//
//-----------------------------------------------------------------------

namespace ARPB2
{
    using System;
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

        public LevelController LevelController;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

        private List<GameObject> PlaneObjects = new List<GameObject>();

        private bool _KeepTracking = true;

        private DetectedPlatform LevelPlatform = null;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking || !_KeepTracking)
            {
                Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.All);
                return;
            }

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to
            // visualize them.
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                GameObject planeObject =
                    Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
                PlaneObjects.Add(planeObject);
            }

            _CheckAreaRequirement();
        }

        private void _CheckAreaRequirement()
        {
            List<DetectedPlane> planes = new List<DetectedPlane>();
            Session.GetTrackables<DetectedPlane>(planes, TrackableQueryFilter.All);
            for (int i = 0; i < planes.Count && _KeepTracking; ++i)
            {
                DetectedPlane plane = planes[i];
                if (_CalculateArea(plane) > MinArea)
                {
                    var session = GameObject.Find("ARCore Device").GetComponent<ARCoreSession>();
                    session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
                    session.OnEnable();
                    _OnPlatformFound(plane);
                    LevelController.PlaceAndyOn(LevelPlatform);
                }
            }
        }

        /// <summary>
        /// We will assume that the center of the polygon is inside it
        /// </summary>
        private float _CalculateArea(DetectedPlane plane)
        {
            float area = 0f;
            Vector3 p1, p2;
            List<Vector3> polygon = new List<Vector3>();
            plane.GetBoundaryPolygon(polygon);

            Vector3 c = plane.CenterPose.position;
            for (int i = 0; i < polygon.Count; ++i)
            {
                p1 = polygon[i];
                p2 = polygon[i + 1 < polygon.Count ? i + 1 : 0];
                area += Math.Abs(p1.x * (p2.z - c.z) + p2.x * (c.z - p1.z) + c.x * (p1.z - p2.z)) / 2;
            }

            return area;
        }

        private void _OnPlatformFound(DetectedPlane plane)
        {
            Utils.ShowAndroidToastMessage("LLEGAMO AL MINIMUM AREA");
            LevelPlatform = new DetectedPlatform(plane);
            _KeepTracking = false;
            for (int i = PlaneObjects.Count - 1; i >= 0; --i)
            {
                GameObject planeObject = PlaneObjects[i];
                DetectedPlaneVisualizer visualizer = planeObject.GetComponent<DetectedPlaneVisualizer>();
                if (!visualizer.VisualizesPlane(plane))
                {
                    visualizer.StopDetection();
                    Destroy(planeObject);
                }
            }

            // Dangerous maybe? Should assign empty list?
            PlaneObjects = null;
        }
    }
}
