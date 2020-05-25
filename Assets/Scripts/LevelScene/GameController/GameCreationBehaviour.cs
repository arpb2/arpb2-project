using UnityEngine;
using System.Collections.Generic;

public class GameCreationBehaviour : MonoBehaviour
{
    public GameObject BoardPrefab;
    public GameObject MainCharacterPrefab;
    public WebViewContainerBehaviour WebViewContainer;


    private PlatformBoardBehaviour board;
    private MainCharacterBehaviour arpb2;

    void Start()
    {
        WebViewContainer.webView.OnMessageReceived += GetComponent<GameControllerBehaviour>().ProcessActions;
    }

    public void BuildGame(LevelSpecification level)
    {
        // Build board
        Debug.Log(">>> Building board from level " + level.ToJSON());
        board = Instantiate(BoardPrefab, transform).GetComponent<PlatformBoardBehaviour>();
        board.Build(level.PlatformRequirements[0]);

        // Locate board elements
        Debug.Log(">>> Board built, locating elements");

        arpb2 = board.LocateElement(MainCharacterPrefab, level.Origin.Coordinate).GetComponent<MainCharacterBehaviour>();
        arpb2.Orientation = level.Origin.Orientation;

        GetComponent<GameControllerBehaviour>().Board = board;
        GetComponent<GameControllerBehaviour>().Player = arpb2;

        Debug.Log(">>> All elements located");
    }
}
