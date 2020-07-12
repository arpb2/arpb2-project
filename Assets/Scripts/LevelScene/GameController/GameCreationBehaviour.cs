using UnityEngine;
using System.Collections.Generic;
using ARPB2;

public class GameCreationBehaviour : MonoBehaviour
{
    public GameObject BoardPrefab;
    public GameObject MainCharacterPrefab;
    public WebViewContainerBehaviour WebViewContainer;
    public GameObject EnergyCellPrefab;
    public GameObject TeleportPadPrefab;
    public GameObject FlagPrefab;

    private PlatformBoardBehaviour board;
    private MainCharacterBehaviour arpb2;

    void Start()
    {
        WebViewContainer.webView.OnMessageReceived += GetComponent<GameControllerBehaviour>().ProcessActions;
    }

    public void ResetBoard()
    {
        if (board) Destroy(board.gameObject);
        board = null;
    }

    public void BuildGame(LevelSpecification level)
    {
        // Build board
        Debug.Log(">>> Building board from level " + level.ToJSON());
        ResetBoard();
        board = Instantiate(BoardPrefab, transform).GetComponent<PlatformBoardBehaviour>();
        board.Build(level.PlatformRequirements[0]);

        // Locate board elements
        Debug.Log(">>> Board built, locating elements");

        arpb2 = board.LocateElement(MainCharacterPrefab, level.Origin.Coordinate).GetComponent<MainCharacterBehaviour>();
        arpb2.Orientation = level.Origin.Orientation;

        foreach (Collectible collectibe in level.Collectibles)
        {
            board.LocateElement(EnergyCellPrefab, collectibe.Coordinate);
        }

        List<GameObject> pads = new List<GameObject>();

        if (level.Pads != null) {
            foreach (Pad pad in level.Pads)
            {
                GameObject teleportPad = board.LocateElement(TeleportPadPrefab, pad.Coordinate);
                pads.Add(teleportPad);
            }

            pads[0].GetComponent<CustomTeleporter>().destinationPad[0] = pads[1].transform;
            pads[1].GetComponent<CustomTeleporter>().destinationPad[0] = pads[0].transform;
        }

        board.LocateElement(FlagPrefab, level.Destination.Coordinate);

        GetComponent<GameControllerBehaviour>().Board = board;
        GetComponent<GameControllerBehaviour>().ARPB2 = arpb2;

        Debug.Log(">>> All elements located");
    }
}
