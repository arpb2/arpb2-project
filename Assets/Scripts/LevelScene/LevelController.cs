//-----------------------------------------------------------------------
//
// Copied from HelloArController.
//
// Here we handle any event that happens
//
//-----------------------------------------------------------------------

namespace ARPB2
{
    using GoogleARCore;
    using System.Collections.Generic;
    using UnityEngine;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class LevelController : MonoBehaviour
    {

        public MainCharacterBehaviour MainCharacter;
        public GameObject DebugArrows;
        public PlatformDetectionStrategy PlatformGenerator;
        public GameObject BoardPrefab;

        private LevelSpecification LevelSpecification;


        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public void Start()
        {
            DebugArrows.SetActive(false);
            LevelSpecification = LevelSpecification.Load("{\"minimal_dimensions\":{\"rows\":2,\"columns\":3}}");
            PlatformGenerator.PlatformRequirements = LevelSpecification.GeneratePlatformRequirements();
            PlatformGenerator.SetOnDetectionFinishedListener(_OnDetectionFinished);
        }

        public void Update()
        {
            _UpdateApplicationLifecycle();
        }

        public void PlaceCharacterOn(DetectedPlatform platform)
        {
            MainCharacter.PlaceCharacter(platform);
            DebugArrows.SetActive(true);
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                Utils.ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                Utils.ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        private void _OnDetectionFinished(List<DetectedPlatform> platforms)
        {
            GameObject board = Instantiate(BoardPrefab, transform);
            board.GetComponent<PlatformBoardBehaviour>().Build(platforms[0]);
        }

    }
}
