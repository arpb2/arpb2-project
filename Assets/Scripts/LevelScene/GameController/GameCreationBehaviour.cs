using UnityEngine;
using System.Collections.Generic;

public class GameCreationBehaviour : MonoBehaviour
{
    public GameObject BoardPrefab;
    public GameObject MainCharacterPrefab;
    public WebViewContainerBehaviour WebViewContainer;
    public GameObject CollectiblePrefab;
    public GameObject TeleportPadPrefab;
    public GameObject FlagPrefab;

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

        arpb2 = board.LocateElement<MainCharacterBehaviour>(MainCharacterPrefab, level.Origin.Coordinate);
        arpb2.Orientation = level.Origin.Orientation;

        foreach (Collectible collectibe in level.Collectibles)
        {
            board.LocateElement<CollectibleBehaviour>(CollectiblePrefab, collectibe.Coordinate);
        }

        List<TeleporterBehaviour> pads = new List<TeleporterBehaviour>();

        foreach (Pad pad in level.Pads)
        {
            TeleporterBehaviour teleportPad = board.LocateElement<TeleporterBehaviour>(TeleportPadPrefab, pad.Coordinate);
            pads.Add(teleportPad);
        }

        pads[0].GetComponent<TeleporterBehaviour>().DestinationPad = pads[1];
        pads[1].GetComponent<TeleporterBehaviour>().DestinationPad = pads[0];

        board.LocateElement(FlagPrefab, level.Destination.Coordinate);

        GetComponent<GameControllerBehaviour>().Board = board;
        GetComponent<GameControllerBehaviour>().Player = arpb2;

        Debug.Log(">>> All elements located");
    }
}
