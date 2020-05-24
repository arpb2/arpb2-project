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
        WebViewContainer.webView.OnMessageReceived += this.ProcessActions;
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
        arpb2.orientation = level.Origin.Orientation;
        arpb2.location = level.Origin.Coordinate;
        Debug.Log(">>> All elements located");
    }


    private void ProcessActions(UniWebView webView, UniWebViewMessage message)
    {
        if (board == null)
        {
            Debug.Log(">>> Board is not yet set, actions won't be processed");
            return;
        }

        WebViewContainer.SetWebViewVisibility(false);

        List<string> actions = new List<string>(message.Args["action"].Split(','));
        foreach (string action in actions)
        {
            switch (action)
            {
                case "move_forward":
                    MoveForward();
                    break;
                case "rotate_left":
                    RotateLeft();
                    break;
                case "rotate_right":
                    RotateRight();
                    break;
                default:
                    Debug.LogError(">>> Cannot parse action " + action);
                    break;
            }
        }
    }

    private void MoveForward()
    {
        Debug.Log(">>> Move forward");

        Coordinate delta = arpb2.orientation.Equals(Orientation.N) ? new Coordinate(0, 1) :
                arpb2.orientation.Equals(Orientation.E) ? new Coordinate(1, 0) :
                arpb2.orientation.Equals(Orientation.S) ? new Coordinate(0, -1) :
                arpb2.orientation.Equals(Orientation.W) ? new Coordinate(-1, 0) : null;

        MovementResult result = board.MoveElement(arpb2.location, delta);

        if (result.Equals(MovementResult.Success))
        {
            arpb2.location += delta;
        }
    }

    private void RotateLeft()
    {
        Debug.Log(">>> Rotate left");
        arpb2.TurnLeft();
    }

    private void RotateRight()
    {
        Debug.Log(">>> Rotate right");
        arpb2.TurnRight();
    }
}
