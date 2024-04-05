using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SpiralMotion_XZPlane : AbstractCircularMotion
{
    [SerializeField, Range(0f, 30f)]
    private float semiaxis_A, semiaxis_B = 2f;
    [SerializeField, Range(0f, 30f)]
    private float decreaseSpeed = 1.5f;

    private float radius;

    void Start()
    {
        radius = Vector3.Distance(transform.position, targetPosition);

        currentSpeed = speed;
        Timing.RunCoroutine(Move());
    }
    protected override IEnumerator<float> Move()
    {
        while (true)
        {
            // relative vector from center to object
            Vector3 relativePos = transform.position - targetPosition;

            // Align rotation to radius direction vector, in order to always face the center object
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;

            // compute new position
            Vector3 newPosition = targetPosition;
            newPosition.x += semiaxis_A * radius * Mathf.Cos(angle);
            newPosition.z += semiaxis_B * radius * Mathf.Sin(angle);
            transform.position = newPosition;

            radius -= decreaseSpeed * Time.deltaTime;
            angle += (clockwiseMotion ? (-1) : 1) * currentSpeed * Time.deltaTime;

            yield return Timing.WaitForOneFrame;
        }
    }
}
