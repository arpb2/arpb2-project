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

    /*
     * Manages the detection of planes for the level.
     */
    public class PlatformDetectionStrategy : MonoBehaviour
    {
        // A prefab for tracking and visualizing detected planes.
        public GameObject DetectedPlanePrefab;
        public List<PlatformRequirement> PlatformRequirements { get; set; }

        private List<DetectedPlane> NewPlanes = new List<DetectedPlane>();
        private List<GameObject> PlaneObjects = new List<GameObject>();
        private bool KeepTracking = true;
        private List<DetectedPlatform> LevelPlatforms = new List<DetectedPlatform>();
        private Action OnDetectionFinishedCallback = null;


        public void Update()
        {
            // Check that motion tracking is tracking, or if tracking has ended
            if (Session.Status == SessionStatus.Tracking && KeepTracking)
            {
                InstantiateNewPlanes();

                CheckPlatformRequirements();
            }
        }

        public void SetOnDetectionFinishedCallback(Action callback)
        {
            OnDetectionFinishedCallback = callback;
        }

        public void StopPlaneTracking()
        {
            KeepTracking = false;
            var session = GameObject.Find("ARCore Device").GetComponent<ARCoreSession>();
            session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
            session.OnEnable(); // This updates the new configuration
        }

        public void StartPlaneTracking()
        {
            KeepTracking = true;
            var session = GameObject.Find("ARCore Device").GetComponent<ARCoreSession>();
            session.SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
            session.OnEnable(); // This updates the new configuration
        }


        private void InstantiateNewPlanes()
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

        private void CheckPlatformRequirements()
        {
            List<DetectedPlane> planes = new List<DetectedPlane>();
            Session.GetTrackables<DetectedPlane>(planes, TrackableQueryFilter.All);

            List<PlatformRequirement> remainingRequirements = PlatformRequirements.FindAll(req => !req.IsSatisfied());
            foreach (PlatformRequirement requirement in remainingRequirements)
            {
                List<DetectedPlane> unassignedPlanes = planes.FindAll(plane => !IsLevelPlatform(plane));
                foreach (DetectedPlane plane in unassignedPlanes)
                {
                    if (requirement.IsMetBy(plane))
                    {
                        OnPlatformFound(plane, requirement);
                        break;
                    }
                }
            }

            bool allRequirementsAreMet = PlatformRequirements.TrueForAll(req => req.IsSatisfied());
            if (allRequirementsAreMet)
            {
                OnDetectionFinished();
            }
        }

        private void OnPlatformFound(DetectedPlane plane, PlatformRequirement requirement)
        {
            var newPlatform = new DetectedPlatform(plane);
            requirement.Platform = newPlatform;
            LevelPlatforms.Add(newPlatform);
        }

        private bool IsLevelPlatform(DetectedPlane plane)
        {
            foreach (var platform in LevelPlatforms)
                if (platform.EqualsPlane(plane))
                    return true;

            return false;
        }

        private void OnDetectionFinished()
        {
            // Remove visual objects of unused planes
            for (int i = PlaneObjects.Count - 1; i >= 0; --i)
            {
                GameObject planeObject = PlaneObjects[i];
                var visualizer = planeObject.GetComponent<DetectedPlaneVisualizer>();
                if (!IsLevelPlatform(visualizer.DetectedPlane))
                {
                    visualizer.StopDetection();
                    Destroy(planeObject);
                }
            }
            PlaneObjects.Clear();

            StopPlaneTracking();

            // And call listener
            OnDetectionFinishedCallback?.Invoke();
        }
    }
}
