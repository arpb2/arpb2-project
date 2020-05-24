using UnityEngine;

public class MainCharacterBehaviour : ElementBehaviour
{
    public Orientation orientation;
    public Coordinate location;

    private Animator AvatarAnimation;


    public void Start()
    {
        AvatarAnimation = GetComponent<Animator>();
    }

    public void DebugMoveForward(float distance)
    {
        MoveForward(distance);
    }

    public MovementResult MoveForward(float distance)
    {
        Debug.Log(">>> MISSING IMPLEMENTATION");
        return MovementResult.Error;
    }

    public void OnMoveForwardFinished(float distance)
    {
        gameObject.transform.position -= gameObject.transform.forward * distance;
        AvatarAnimation.SetBool("IsAdvancing", false);
    }

    public void TurnRight()
    {
        gameObject.transform.Rotate(0, 90, 0);

        Orientation newOrientation = orientation.Equals(Orientation.N) ? Orientation.E :
                orientation.Equals(Orientation.E) ? Orientation.S :
                orientation.Equals(Orientation.S) ? Orientation.W :
                Orientation.N;

        orientation = newOrientation;
    }

    public void TurnLeft()
    {
        gameObject.transform.Rotate(0, -90, 0);

        Orientation newOrientation = orientation.Equals(Orientation.N) ? Orientation.W :
                orientation.Equals(Orientation.W) ? Orientation.S :
                orientation.Equals(Orientation.S) ? Orientation.E :
                Orientation.N;

        orientation = newOrientation;
    }

}
