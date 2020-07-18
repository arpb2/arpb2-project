using System.Collections.Generic;
using UnityEngine;

class ExecuteCodeBehaviour : MonoBehaviour
{
    public List<string> Actions;
    public bool SendCommand = false;
    public string Command;
    public GameControllerBehaviour GameController;

    public void ExecuteActions()
    {
        string wholePath;

        if (SendCommand) wholePath = Command;
        else wholePath = "arpb2?action=" + string.Join("&action=", Actions);
        
        UniWebViewMessage message = new UniWebViewMessage("uniwebview://" + wholePath);
        GameController.ProcessActions(null, message);
    }
}