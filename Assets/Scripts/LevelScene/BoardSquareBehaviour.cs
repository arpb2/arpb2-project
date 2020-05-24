using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Components that holds the BoardSquare logic 
 */

public class BoardSquareBehaviour : MonoBehaviour
{

    public ElementBehaviour element;

    public bool IsFree()
    {
        return element == null;
    }

    public void SetElement(ElementBehaviour element)
    {
        this.element = element;
    }

    public ElementBehaviour RemoveElement()
    {
        var element = this.element;
        this.element = null;
        return element;
    }

}
