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

        private List<PlatformRequirement> platformRequirements;
        public List<PlatformRequirement> PlatformRequirements
        {
            get { return platformRequirements; }
            set { platformRequirements = value; }
        }


        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> NewPlanes = new List<DetectedPlane>();

        private List<GameObject> PlaneObjects = new List<GameObject>();

        private bool KeepTracking = true;

        private List<DetectedPlatform> LevelPlatforms = new List<DetectedPlatform>();

        private Action<List<DetectedPlatform>> OnDetectionFinishedCallback = null;


        public void Update()
        {
            // Check that motion tracking is tracking, or if tracking has ended
            if (Session.Status == SessionStatus.Tracking && KeepTracking)
            {
                _InstantiateNewPlanes();

                _CheckPlatformRequirements();
            }
        }

        public void SetOnDetectionFinishedListener(Action<List<DetectedPlatform>> callback)
        {
            OnDetectionFinishedCallback = callback;
        }

        public void StopTrackingPlanes()
        {
            KeepTracking = false;
            var session = GameObject.Find("ARCore Device").GetComponent<ARCoreSession>();
            session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
            session.OnEnable(); // This updates the new configuration
        }


        private void _InstantiateNewPlanes()
        {
            // Iterate over planes found in this frame and instantiate corresponding GameObjects to
            // visualize them.
            Session.GetTrackables<DetectedPlane>(NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < NewPlanes.Count; i++)
            {
                GameObject planeObject =
                    Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(NewPlanes[i]);
                PlaneObjects.Add(planeObject);
            }
        }

        private void _CheckPlatformRequirements()
        {
            List<DetectedPlane> planes = new List<DetectedPlane>();
            Session.GetTrackables<DetectedPlane>(planes, TrackableQueryFilter.All);

            // For each remaining requirement
            foreach (PlatformRequirement requirement in platformRequirements.FindAll(req => !req.IsSatisfied()))
            {
                // For each plane which has not been "chosen" yet
                foreach(DetectedPlane plane in planes.FindAll(plane => !_IsLevelPlatform(plane)))
                {
                    if (requirement.IsMetBy(plane))
                    {
                        _OnPlatformFound(plane, requirement);
                        break;
                    }
                }
            }

            bool allRequirementsAreMet = platformRequirements.TrueForAll(req => req.IsSatisfied());
            if (allRequirementsAreMet)
            {
                _OnDetectionFinished();
            }
        }

        private void _OnPlatformFound(DetectedPlane plane, PlatformRequirement requirement)
        {
            var newPlatform = new DetectedPlatform(plane);
            requirement.Platform = newPlatform;
            newPlatform.IsInitial = requirement.IsInitial;
            LevelPlatforms.Add(newPlatform);
            Utils.ShowAndroidToastMessage(String.Format("Platform found {0}/{1}", LevelPlatforms.Count, PlatformRequirements.Count));
        }

        private bool _IsLevelPlatform(DetectedPlane plane)
        {
            foreach (var platform in LevelPlatforms)
                if (platform.EqualsPlane(plane))
                    return true;

            return false;
        }

        private void _OnDetectionFinished()
        {
            //Utils.ShowAndroidToastMessage("All platforms found");
            
            // Remove visual objects of unused planes
            for (int i = PlaneObjects.Count - 1; i >= 0 && false; --i)
            {
                GameObject planeObject = PlaneObjects[i];
                var visualizer = planeObject.GetComponent<DetectedPlaneVisualizer>();
                if (! _IsLevelPlatform(visualizer.DetectedPlane))
                {
                    visualizer.StopDetection();
                    Destroy(planeObject);
                }
            }
            PlaneObjects.Clear();
            
            StopTrackingPlanes();

            // And call listener
            OnDetectionFinishedCallback?.Invoke(LevelPlatforms);
        }
    }
}
