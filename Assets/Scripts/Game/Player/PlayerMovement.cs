using MEC;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    protected float defaultSpeed;
    protected float speed;
    [SerializeField]
    private float currentSpeed;
    [SerializeField, Range(0f, 100f)]
    private float semiaxis_A, semiaxis_B = 2f;
    private Vector3 movementDirection = Vector3.zero;
    public bool clockwiseMotion=true;
    private float angle = 0.0f;
    public Animator anim;
    private bool startG=false;
    public float Acceleration=0.1f;
    public float deceleration = 0.3f;
    public bool isAnimStart=false;
    public GameObject displayAmmo;
    [HideInInspector]
    public Vector2 inputVector;
    [SerializeField]
    private GameObject defend;

    protected void Start()
    {
        speed = defaultSpeed;
        Timing.RunCoroutine(Move().CancelWith(gameObject));
    }
    protected  IEnumerator<float> Move()
    {
        while (true)
        {
            // Check if there is movement direction set
            if (movementDirection != Vector3.zero && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && startG==true)
            {
                //Acceleration
                if (currentSpeed < speed && inputVector != Vector2.zero)
                {
                    currentSpeed += Acceleration * Time.deltaTime;
                }
            
                // Calculate new position based on movement direction
                Vector3 newPosition = transform.position + movementDirection * currentSpeed * Time.deltaTime;
                newPosition.x = semiaxis_A * Mathf.Cos(angle);
                newPosition.z = semiaxis_B * Mathf.Sin(angle);
                transform.position = newPosition;

                // Rotate towards the target
                Vector3 lookDirection = Vector3.zero - transform.position;

                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = rotation;

                // Increment angle
                angle += (clockwiseMotion ? (-1) : 1) * currentSpeed * Time.deltaTime;
            }
         
            yield return Timing.WaitForOneFrame;
        }
    }
    // Method to set movement direction from external script (player movement)
    public void SetMovementDirection(Vector3 inputVector)
    {  
        // If input right movement clocwise
        if (inputVector.x > 0f && clockwiseMotion)
        {
            if (!isAnimStart)
            {
                isAnimStart = true;
                defend.SetActive(false);
                clockwiseMotion = false;
                startG = true;
                GameController.Instance.ImgAmmoActivated();
                anim.SetBool("FirstInput", true);
                FoamEffect.Instance.StartFoam(isAnimStart);
              
            }
            else
            {
                currentSpeed = 0;
              //  startG = true;
                anim.Play("TurnLeft");
                clockwiseMotion = false;
            }
           
        }
        // if input left movement counterclockwise
        else if (inputVector.x < 0f && !clockwiseMotion&& startG==true)
        {
            currentSpeed=0;
            anim.Play("TurnRight");
            clockwiseMotion = true;
        }

        // direction of movement
        movementDirection = inputVector;
    }
    protected IEnumerator<float> DecelerationCo()
    {
        while (currentSpeed > 0 && inputVector == Vector2.zero)
        {
            // Riduci gradualmente la velocità
            currentSpeed -= deceleration * Time.deltaTime;
            // Attendi il prossimo frame
            yield return Timing.WaitForOneFrame;
        }
        if (currentSpeed < 0)
        {
            currentSpeed = 0f;
            Timing.KillCoroutines("Deceleration");
            SetMovementDirection(Vector3.zero);
        }
        Timing.KillCoroutines("Deceleration");
    }
    private void OnDrawGizmos()//Draw Gizmos for test
    {
        // calculate CenterOfEllipse
        Vector3 ellipseCenter = Vector3.zero;

        // drawPath
        Gizmos.color = Color.green;
        float angleStep = 0.1f;
        float currentAngle = 0f;
        Vector3 lastPosition = Vector3.zero;
        while (currentAngle <= 2 * Mathf.PI)
        {
            float x = semiaxis_A * Mathf.Cos(currentAngle) + ellipseCenter.x;
            float z = semiaxis_B * Mathf.Sin(currentAngle) + ellipseCenter.z;

            Vector3 currentPosition = new Vector3(x, ellipseCenter.y, z);
            if (currentAngle > 0)
            {
                Gizmos.DrawLine(lastPosition, currentPosition);
            }
            lastPosition = currentPosition;
            currentAngle += angleStep;
        }
    }
}