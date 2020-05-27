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
            LevelSpecification = LevelSpecification.LoadDebug();
            Debug.Log(">>> Level JSON: " + LevelSpecification.ToJSON());
            Camera.enabled = true;
            GetComponent<ScanLevelBehaviour>().StartScanning();
        }
    }
}