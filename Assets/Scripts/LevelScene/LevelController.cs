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


        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public void Start()
        {
            DebugArrows.SetActive(false);
            PlatformGenerator.PlatformRequirements = new List<PlatformRequirement>(new PlatformRequirement[] {
                new PlatformRequirement(1, null),
                new PlatformRequirement(1, null)
            });
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
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
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

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        private void _OnDetectionFinished(DetectedPlatform initPlatform)
        {
            PlaceCharacterOn(initPlatform);
        }

    }
}
