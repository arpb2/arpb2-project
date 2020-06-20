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

    public bool wonLevel;

    private GameObject winningPanel;

    private void Start()
    {
        wonLevel = false;
        winningPanel = GameObject.Find("WinningPanel");
        winningPanel.SetActive(false);
    }

    private void Update()
    {
        winningPanel.SetActive(wonLevel);
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

            yield return new WaitForSeconds(1.5f);
        }
        IsExecutingCode = false;
    }

    private void MoveForward()
    {
        Debug.Log(">>> Move forward");

        Coordinate destination = arpb2.Location + OrientationToCoords(arpb2.Orientation);
        MovementResult result = board.MoveElement(arpb2, destination);

        if (result.Equals(MovementResult.Success))
        {
            arpb2.MoveForward();
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

    private Coordinate OrientationToCoords(Orientation orientation)
    {
        return orientation.Equals(Orientation.N) ? new Coordinate(0, 1) :
                orientation.Equals(Orientation.E) ? new Coordinate(1, 0) :
                orientation.Equals(Orientation.S) ? new Coordinate(0, -1) :
                orientation.Equals(Orientation.W) ? new Coordinate(-1, 0) : null;
    }
}
