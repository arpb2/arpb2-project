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

        public void OnLoadNewLevelEvent(UniWebView webView, UniWebViewMessage message)
        {
            if (!message.Path.Equals("arpb2/level"))
            {
                Debug.Log("not a change level event");
                return;
            }
            Debug.Log("");
            LevelSpecificationRequester.Get(this, int.Parse(message.Args["next"]), OnLevelSpecificationRetrieved, OnLevelSpecificationFail);
        }

        private void OnLevelSpecificationRetrieved(LevelSpecification level)
        {
            OnLevelLoaded(level);
        }

        private void OnLevelSpecificationFail(string error)
        {
            Debug.Log("Error retrieving level specification: " + error);
        }
    }
}