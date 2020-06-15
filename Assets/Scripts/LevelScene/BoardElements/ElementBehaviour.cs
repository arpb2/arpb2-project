using UnityEngine;

public enum ElementType
{
    Pad,
    Collectible,
    MainCharacter
}

public class ElementBehaviour : MonoBehaviour
{
    public bool Solid = false;

    public ElementType Type;

    public bool AutomaticInteraction = true;

    [HideInInspector]
    public BoardSquareBehaviour BoardSquare;

    public virtual void InteractWith(ElementBehaviour other) { }

    public void MoveTo(BoardSquareBehaviour newSquare)
    {
        BoardSquare.RemoveElement(this);
        newSquare.SetElement(this);

        foreach (ElementBehaviour element in BoardSquare.Elements)
            if (this != element && element.AutomaticInteraction)
                element.InteractWith(this);
    }
}
