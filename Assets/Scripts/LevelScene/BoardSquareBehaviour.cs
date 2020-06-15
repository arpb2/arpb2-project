using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Components that holds the BoardSquare logic 
 */

public class BoardSquareBehaviour : MonoBehaviour
{

    public List<ElementBehaviour> Elements = new List<ElementBehaviour>();
    public TextMesh DebugText;
    public Coordinate Location;

    public bool IsFree()
    {
        foreach (ElementBehaviour e in Elements)
            if (e.Solid) return false;

        return true;
    }

    public void SetElement(ElementBehaviour newElement)
    {
        newElement.BoardSquare = this;
        this.Elements.Add(newElement);
    }

    public void RemoveElement(ElementBehaviour element)
    {
        this.Elements.Remove(element);
    }

    public void SetDebugText(string text)
    {
        DebugText.gameObject.SetActive(true);
        DebugText.text = text;
    }
}
