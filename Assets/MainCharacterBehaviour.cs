using System.Collections;
using UnityEngine;

public class MainCharacterBehaviour : ElementBehaviour
{
    public Orientation Orientation;

    public bool ExecutingAction { private set; get; }

    private Animator animator;

    public float RotateTime = 1.0f;
    public float RotateDegrees = 90.0f;
    private bool rotating = false;

    public int Points { get; private set; }


    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {

        if (animator.GetBool("RotateRight_Anim") && !rotating)
        {
            StartCoroutine(Rotate(transform, gameObject.transform, Vector3.up, RotateDegrees, RotateTime));
            OnTurnRightFinished();
        }

        if (animator.GetBool("RotateLeft_Anim") && !rotating)
        {
            StartCoroutine(Rotate(transform, gameObject.transform, Vector3.up, -RotateDegrees, RotateTime));
            OnTurnLeftFinished();
        }
    }

    public void AddPoint()
    {
        Points++;
    }

    public void ResetPoints()
    {
        Points = 0;
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

    private IEnumerator Rotate(Transform camTransform, Transform targetTransform, Vector3 rotateAxis, float degrees, float totalTime)
    {
        if (rotating)
            yield return null;
        rotating = true;

        Quaternion startRotation = camTransform.rotation;
        Vector3 startPosition = camTransform.position;
        // Get end position;
        transform.RotateAround(targetTransform.position, rotateAxis, degrees);
        Quaternion endRotation = camTransform.rotation;
        Vector3 endPosition = camTransform.position;
        camTransform.rotation = startRotation;
        camTransform.position = startPosition;

        float rate = degrees / totalTime;
        //Start Rotate
        for (float i = 0.0f; Mathf.Abs(i) < Mathf.Abs(degrees); i += Time.deltaTime * rate)
        {
            camTransform.RotateAround(targetTransform.position, rotateAxis, Time.deltaTime * rate);
            yield return null;
        }

        camTransform.rotation = endRotation;
        camTransform.position = endPosition;
        rotating = false;
    }
}
