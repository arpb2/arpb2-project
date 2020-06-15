using UnityEngine;

public class GoalBehaviour : ElementBehaviour
{
    public override void InteractWith(ElementBehaviour other)
    {
        Debug.Log(">>> Level complete!");
    }
}
