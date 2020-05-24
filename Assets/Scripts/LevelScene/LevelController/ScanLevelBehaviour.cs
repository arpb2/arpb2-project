using UnityEngine;


namespace ARPB2
{
    public class ScanLevelBehaviour : MonoBehaviour
    {
        public PlatformDetectionStrategy PlatformGenerator;
        public GameCreationBehaviour GameCreation;

        void Start()
        {
            PlatformGenerator.StopPlaneTracking();
            PlatformGenerator.SetOnDetectionFinishedCallback(OnScanningFinished);
        }

        public void StartScanning()
        {
            LevelSpecification level = GetComponent<LoadLevelBehaviour>().LevelSpecification;
            PlatformGenerator.PlatformRequirements = level.PlatformRequirements;
            PlatformGenerator.StartPlaneTracking();
        }

        public void OnScanningFinished()
        {
            Debug.Log(">>> Scan finished");
            GameCreation.BuildGame(GetComponent<LoadLevelBehaviour>().LevelSpecification);
        }

    }
}