using UnityEngine;
using ARPB2;

public class DebugLevelController : LoadLevelBehaviour
{
    public GameCreationBehaviour GameCreation;

    public bool GetLevelFromServer = true;

    public void Start()
    {
        Debug.Log(">>> DebugLevelController starts");
        LoadNewLevel(1);
    }

    override public void LoadNewLevel(int levelNo)
    {
        GameCreation.ResetBoard();

        if (GetLevelFromServer)
            LevelSpecificationRequester.Get(this, levelNo, OnLevelLoaded);
        else
            OnLevelLoaded(LevelSpecification.LoadDebug());
    }

    private void OnLevelLoaded(LevelSpecification level) {
        LevelSpecification = level;
        level.PlatformRequirements[0].Platform = new DebugPlatform();
        GameCreation.BuildGame(level);    
    }
}

