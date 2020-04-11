using System;
using UnityEngine;
using UnityEngine.UI;


namespace ARPB2
{
    public class LoadingScreenBehaviour : MonoBehaviour
    {
        public Text LoadingText;
    
        public LevelSpecification LevelSpecification { private set; get; }
        private Action<LevelSpecification> OnLevelLoaded = null;


        void Start()
        {
            LevelSpecificationRequester.Get(this, 1, OnLevelSpecificationRetrieved, OnLevelSpecificationFail);
        }

        void Update()
        {
            int seconds = (int)(Time.time / 1000);
            switch (seconds % 3)
            {
                case 0:
                    LoadingText.text = "Loading.";
                    break;
                case 1:
                    LoadingText.text = "Loading..";
                    break;
                case 2:
                    LoadingText.text = "Loading...";
                    break;
            }
        }

        public void SetOnLevelLoadedCallback(Action<LevelSpecification> callback)
        {
            OnLevelLoaded = callback;
        }

        public void Finish()
        {
            gameObject.SetActive(false);
        }

        private void OnLevelSpecificationRetrieved(LevelSpecification level)
        {
            LevelSpecification = level;
            Finish();
            OnLevelLoaded?.Invoke(level);
        }

        private void OnLevelSpecificationFail(string error)
        {
            Debug.Log("Error retrieving level specification: " + error);
        }
    }
}