using System.Collections.Generic;
using UnityEngine;

class ExecuteCodeBehaviour : MonoBehaviour
{
    public List<string> Actions;
    public GameControllerBehaviour GameController;

    public void ExecuteActions()
    {
        string args = "action=" + string.Join("&action=", Actions);
        UniWebViewMessage message = new UniWebViewMessage("uniwebview://arpb2?" + args);
        GameController.ProcessActions(null, message);
    }
}