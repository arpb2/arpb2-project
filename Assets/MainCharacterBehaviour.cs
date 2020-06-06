using UnityEngine;

public class MainCharacterBehaviour : ElementBehaviour
{
    public float RotationSpeed = 40f;
    public Orientation Orientation;

    public bool ExecutingAction { private set; get; }

    private Animator animator;
    private float previousAngle;
    private float limitRotation;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (animator.GetBool("RotateRight_Anim"))
        {
            var delta = RotationSpeed * Time.fixedDeltaTime;

            Vector3 newAngle = transform.eulerAngles;
            newAngle[1] = Mathf.Min(newAngle[1] + delta, previousAngle + 90f);

            transform.eulerAngles = newAngle;

            if (newAngle[1] >= previousAngle + 90f) OnTurnRightFinished();
        }
        else if (animator.GetBool("RotateLeft_Anim"))
        {
            Debug.Log(">>> rotating left...");
            var delta = RotationSpeed * Time.fixedDeltaTime;

            Vector3 newAngle = transform.eulerAngles;
            newAngle[1] -= delta;
            if (newAngle[1] < 0) newAngle[1] += 360f;
            newAngle[1] = Mathf.Max(newAngle[1], limitRotation);

            transform.eulerAngles = newAngle;

            if (transform.eulerAngles[1] <= limitRotation) OnTurnLeftFinished();
        }

    }

    public void MoveForward()
    {
        ExecutingAction = true;
        animator.SetBool("Walk_Anim", true);
    }

    public void OnMoveForwardFinished()
    {
        ExecutingAction = false;
        animator.SetBool("Walk_Anim", false);
    }

    public void TurnRight()
    {
        ExecutingAction = true;
        previousAngle = transform.eulerAngles[1];
        animator.SetBool("RotateRight_Anim", true);

        Orientation newOrientation = Orientation.Equals(Orientation.N) ? Orientation.E :
                Orientation.Equals(Orientation.E) ? Orientation.S :
                Orientation.Equals(Orientation.S) ? Orientation.W :
                Orientation.N;

        Orientation = newOrientation;
    }

    public void OnTurnRightFinished()
    {
        ExecutingAction = false;
        animator.SetBool("RotateRight_Anim", false);
    }

    public void TurnLeft()
    {
        ExecutingAction = true;
        limitRotation = transform.eulerAngles[1] - 90f;
        if (limitRotation < 0) limitRotation += 360f;
        animator.SetBool("RotateLeft_Anim", true);

        Orientation newOrientation = Orientation.Equals(Orientation.N) ? Orientation.W :
                Orientation.Equals(Orientation.W) ? Orientation.S :
                Orientation.Equals(Orientation.S) ? Orientation.E :
                Orientation.N;

        Orientation = newOrientation;
    }

    public void OnTurnLeftFinished()
    {
        ExecutingAction = false;
        animator.SetBool("RotateLeft_Anim", false);
    }


}
