using UnityEngine;

public class DebugLevelController : MonoBehaviour
{
    public GameCreationBehaviour GameCreation;

    public void Start()
    {
        Debug.Log(">>> DebugLevelController starts");
        LevelSpecification level = LevelSpecification.LoadDebug();
        level.PlatformRequirements[0].Platform = new DebugPlatform();
        GameCreation.BuildGame(level);
    }
}

