using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPB2;

public class GameControllerBehaviour : MonoBehaviour
{
    public WebViewContainerBehaviour WebViewContainer;

    public LoadLevelBehaviour LevelLoader;

    [HideInInspector]
    public PlatformBoardBehaviour Board;

    [HideInInspector]
    public MainCharacterBehaviour ARPB2;

    [HideInInspector]
    public bool WonLevel;
    public WinningPanelBehaviour WinningPanel;

    private bool IsExecutingCode = false;

    private void Start()
    {
        WonLevel = false;
        WinningPanel.Hide();
    }

    public void SetAsWon()
    {
        WonLevel = true;
        var collectibles = LevelLoader.LevelSpecification.Collectibles;
        int collectiblesNo = collectibles != null ? collectibles.Count : 0;
        WinningPanel.ShowWithPoints(3 - collectiblesNo + ARPB2.Points);
    }
    
    public void ProcessActions(UniWebView webView, UniWebViewMessage message)
    {
        // Close UI
        WebViewContainer.SetWebViewVisibility(false);

        if (message.Path.Equals("arpb2/level"))
        {
            Debug.Log(">>> Next level: " + message.Args["next"]);
            WonLevel = false;
            WinningPanel.Hide();
            LevelLoader.LoadNewLevel(int.Parse(message.Args["next"]));
        }
        else
        {
            if (!message.Path.Equals("arpb2"))
            {
                Debug.Log("not an actions path");
                return;
            }
            if (Board == null)
            {
                Debug.Log(">>> Board is not yet set, actions won't be processed");
                return;
            }

            List<string> actions = new List<string>(message.Args["action"].Split(','));
            StartCoroutine(ExecuteActions(actions));
        }
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

        Coordinate destination = ARPB2.Location + OrientationToCoords(ARPB2.Orientation);
        MovementResult result = Board.MoveElement(ARPB2, destination);

        if (result.Equals(MovementResult.Success))
        {
            ARPB2.MoveForward();
        }
    }

    private void RotateLeft()
    {
        Debug.Log(">>> Rotate left");
        ARPB2.TurnLeft();
    }

    private void RotateRight()
    {
        Debug.Log(">>> Rotate right");
        ARPB2.TurnRight();
    }

    private Coordinate OrientationToCoords(Orientation orientation)
    {
        return orientation.Equals(Orientation.N) ? new Coordinate(0, 1) :
                orientation.Equals(Orientation.E) ? new Coordinate(1, 0) :
                orientation.Equals(Orientation.S) ? new Coordinate(0, -1) :
                orientation.Equals(Orientation.W) ? new Coordinate(-1, 0) : null;
    }
}
