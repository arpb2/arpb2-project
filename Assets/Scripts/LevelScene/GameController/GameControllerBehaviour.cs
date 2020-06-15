using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
    public WebViewContainerBehaviour WebViewContainer;

    // We don't want these on the Unity Inspector
    public PlatformBoardBehaviour Board { set => board = value; get => board; }
    private PlatformBoardBehaviour board;

    public MainCharacterBehaviour Player { set => arpb2 = value; get => arpb2; }
    private MainCharacterBehaviour arpb2;
    private bool IsExecutingCode = false;
    private bool IsExecutingCommand = false;

    public bool wonLevel;

    private void Start()
    {
        wonLevel = false;
    }

    public void ProcessActions(UniWebView webView, UniWebViewMessage message)
    {
        if (board == null)
        {
            Debug.Log(">>> Board is not yet set, actions won't be processed");
            return;
        }

        WebViewContainer.SetWebViewVisibility(false);

        // TODO: Refactor this with Factory + Command Patterns
        List<string> actions = new List<string>(message.Args["action"].Split(','));
        StartCoroutine(ExecuteActions(actions));
    }

    private IEnumerator ExecuteActions(List<string> actions)
    {
        IsExecutingCode = true;
        foreach (string action in actions)
        {
            IsExecutingCommand = true;
            switch (action)
            {
                case "move_forward":
                    StartCoroutine(MoveForward());
                    break;
                case "rotate_left":
                    StartCoroutine(RotateLeft());
                    break;
                case "rotate_right":
                    StartCoroutine(RotateRight());
                    break;
                case "interact":
                    StartCoroutine(DoInteraction());
                    break;
                default:
                    Debug.LogError(">>> Cannot parse action " + action);
                    IsExecutingCommand = false;
                    break;
            }
            yield return new WaitWhile(() => IsExecutingCommand);
        }
        IsExecutingCode = false;
    }

    private IEnumerator MoveForward()
    {
        Debug.Log(">>> Move forward");

        Coordinate destination = arpb2.BoardSquare.Location + OrientationToCoords(arpb2.Orientation);
        MovementResult result = board.CheckMovementResult(arpb2, destination);

        if (result.Equals(MovementResult.Success))
        {
            arpb2.MoveForward();
            yield return new WaitWhile(() => arpb2.ExecutingAction);
            board.MoveElement(arpb2, destination);
        }
        IsExecutingCommand = false;
    }

    private IEnumerator DoInteraction()
    {
        arpb2.DoInteraction();
        yield return new WaitForSeconds(1.5f);
        IsExecutingCommand = false;
    }

    private IEnumerator RotateLeft()
    {
        arpb2.TurnLeft();
        yield return new WaitWhile(() => arpb2.ExecutingAction);
        IsExecutingCommand = false;
    }

    private IEnumerator RotateRight()
    {
        Debug.Log(">>> Rotate right");
        arpb2.TurnRight();
        yield return new WaitWhile(() => arpb2.ExecutingAction);
        IsExecutingCommand = false;
    }

    private Coordinate OrientationToCoords(Orientation orientation)
    {
        return orientation.Equals(Orientation.N) ? new Coordinate(0, 1) :
                orientation.Equals(Orientation.E) ? new Coordinate(1, 0) :
                orientation.Equals(Orientation.S) ? new Coordinate(0, -1) :
                orientation.Equals(Orientation.W) ? new Coordinate(-1, 0) : null;
    }
}
