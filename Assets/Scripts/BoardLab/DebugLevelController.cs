using UnityEngine;
using ARPB2;

public class DebugLevelController : LoadLevelBehaviour
{
    public GameCreationBehaviour GameCreation;

    public void Start()
    {
        Debug.Log(">>> DebugLevelController starts");
        LoadNewLevel(1);
    }

    override public void LoadNewLevel(int levelNo)
    {
        GameCreation.ResetBoard();
        LevelSpecification level = LevelSpecification.LoadDebug(levelNo);
        level.PlatformRequirements[0].Platform = new DebugPlatform();
        GameCreation.BuildGame(level);    
    }
}

