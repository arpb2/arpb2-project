using UnityEngine;


namespace ARPB2
{
    public class LoadLevelBehaviour : MonoBehaviour
    {
        public LoadingScreenBehaviour LoadingScreen;
        public Camera Camera;

        public LevelSpecification LevelSpecification { private set; get; }

        void Start()
        {
            //Camera.enabled = false;
            LoadingScreen.gameObject.SetActive(true);
            LoadingScreen.SetOnLevelLoadedCallback(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelSpecification level)
        {
            Debug.Log(">>> Level is loaded :)");
            Debug.Log(">>> Level JSON: " + LoadingScreen.LevelSpecification.ToJSON());
            LevelSpecification = LevelSpecification.LoadDebug();
            Camera.enabled = true;
            GetComponent<ScanLevelBehaviour>().StartScanning();
        }
    }
}