using UnityEngine;

public class MainCharacterBehaviour : ElementBehaviour
{
    public Orientation Orientation;

    public bool ExecutingAction { private set; get; }

    private Animator animator;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveForward()
    {
        Debug.Log(">>> ARPB2 moves forward");
        ExecutingAction = true;
        animator.SetBool("IsWalking", true);
    }

    public void OnMoveForwardFinished()
    {
        Debug.Log(">>> ARPB2 finished moving forward");
        ExecutingAction = false;
        animator.SetBool("IsWalking", false);
    }

    public void TurnRight()
    {
        ExecutingAction = true;
        animator.SetBool("IsRotatingRight", true);

        Orientation newOrientation = Orientation.Equals(Orientation.N) ? Orientation.E :
                Orientation.Equals(Orientation.E) ? Orientation.S :
                Orientation.Equals(Orientation.S) ? Orientation.W :
                Orientation.N;

        Orientation = newOrientation;
    }

    public void OnTurnRightFinished()
    {
        ExecutingAction = false;
        animator.SetBool("IsRotatingRight", false);
    }

    public void TurnLeft()
    {
        ExecutingAction = true;
        animator.SetBool("IsRotatingLeft", true);

        Orientation newOrientation = Orientation.Equals(Orientation.N) ? Orientation.W :
                Orientation.Equals(Orientation.W) ? Orientation.S :
                Orientation.Equals(Orientation.S) ? Orientation.E :
                Orientation.N;

        Orientation = newOrientation;
    }

    public void OnTurnLeftFinished()
    {
        ExecutingAction = false;
        animator.SetBool("IsRotatingLeft", false);
    }


}
