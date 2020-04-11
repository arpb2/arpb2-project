using UnityEngine;


namespace ARPB2
{
    public class ScanLevelBehaviour : MonoBehaviour
    {
        public PlatformDetectionStrategy PlatformGenerator;
        public GameObject BoardPrefab;

        void Start()
        {
            PlatformGenerator.StopPlaneTracking();
            PlatformGenerator.SetOnDetectionFinishedCallback(OnScanningFinished);
        }

        public void StartScanning()
        {
            LevelSpecification level = GetComponent<LoadLevelBehaviour>().LevelSpecification;
            PlatformGenerator.PlatformRequirements = level.GeneratePlatformRequirements();
            PlatformGenerator.StartPlaneTracking();
        }

        public void OnScanningFinished()
        {
            Debug.Log(">>> Scan finished");
            LevelSpecification level = GetComponent<LoadLevelBehaviour>().LevelSpecification;
            PlatformBoardBehaviour board = Instantiate(BoardPrefab, transform).GetComponent<PlatformBoardBehaviour>();
            board.Build(PlatformGenerator.PlatformRequirements[0]);
            Debug.Log(">>> Board built, locating elements at level " + level.ToJSON());
            board.LocateElements(level);
        }

    }
}