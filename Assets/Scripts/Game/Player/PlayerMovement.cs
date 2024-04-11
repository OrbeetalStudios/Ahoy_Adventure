using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    [SerializeField, Range(0f, 100f)]
    private float semiaxis_A, semiaxis_B = 2f;
    [SerializeField]
    private GameObject model;
    private Vector3 movementDirection = Vector3.zero;

    private void Start()
    {
        Timing.RunCoroutine(Move());
    }
    protected  IEnumerator<float> Move()
    {
        while (true)
        {
            // Check if there is movement direction set
            if (movementDirection != Vector3.zero)
            {
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
    public void SetMovementDirection(Vector2 inputVector)
    {
        // Normalize Input Vector
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
        if (transform.position.z < 0)
        {
            // If input right movement clocwise
            if (inputVector.x > 0f && clockwiseMotion)
            {
                model.transform.Rotate(0, 180, 0);
                clockwiseMotion = false;
            }
            // if input left movement counterclockwise
            else if (inputVector.x < 0f && !clockwiseMotion)
            {
                model.transform.Rotate(0, -180, 0);
                clockwiseMotion = true;
            }
        }
        else if (transform.position.z > 0)
        {
            if (inputVector.x > 0f && !clockwiseMotion)
            {
                model.transform.Rotate(0, -180, 0);
                clockwiseMotion = true;
            }
            // if input left movement counterclockwise
            else if (inputVector.x < 0f && clockwiseMotion)
            {
                model.transform.Rotate(0, 180, 0);
                clockwiseMotion = false;
            }
        }


        // direction of movement
        movementDirection = direction;
    }


    private void OnDrawGizmos()//Draw Gizmos for test
    {
      

        // calculate CenterOFEllipse
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
