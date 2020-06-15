using UnityEngine;

public class TeleporterBehaviour : ElementBehaviour
{
    [HideInInspector]
    public TeleporterBehaviour DestinationPad;

    // Add a sound component if you want the teleport playing a sound when teleporting
    public AudioSource TeleportSound;

    public override void InteractWith(ElementBehaviour other)
    {
        Teleport(other.gameObject);
        other.MoveTo(DestinationPad.BoardSquare);
    }

    void Teleport(GameObject subject)
    {
        subject.transform.position = DestinationPad.transform.position;

        if (TeleportSound != null) TeleportSound.Play();
    }
}
