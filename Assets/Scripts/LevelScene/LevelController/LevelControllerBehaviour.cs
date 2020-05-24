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
    using UnityEngine;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

    public class LevelControllerBehaviour : MonoBehaviour
    {
        public GameObject DebugArrows;

        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        private bool isQuitting = false;

        private PlatformBoardBehaviour board;

        public void Start()
        {
            Debug.Log(">>> LevelController starts");
            DebugArrows.SetActive(false);
        }

        public void Update()
        {
            UpdateApplicationLifecycle();
        }

        /*
         * Check and update the application lifecycle.
         */
        private void UpdateApplicationLifecycle()
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

            if (isQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                Utils.ShowAndroidToastMessage("Camera permission is needed to run this application.");
                isQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                Utils.ShowAndroidToastMessage("ARCore encountered a problem connecting. Please start the app again.");
                isQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        private void DoQuit()
        {
            Application.Quit();
        }

    }
}
