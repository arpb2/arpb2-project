using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : ElementBehaviour
{
    public override void InteractWith(ElementBehaviour other)
    {
        if (other.Type == ElementType.MainCharacter)
        {
            MainCharacterBehaviour arpb2 = (MainCharacterBehaviour)other;
            // arpb2.GrabCollectible(this);

            BoardSquare.RemoveElement(this);
            Destroy(gameObject);
            Debug.Log(">>> Collectible grabbed");
        }
    }
}
