using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemyMovement : AbstractMotionToTarget
{
    [SerializeField] private float distanceTraveled;
    private float distanceThreshold = 150f;

    protected void OnEnable()
    {
        moveToTargetHandle = Timing.RunCoroutine(Move().CancelWith(gameObject));
    }
    protected IEnumerator<float> ReturnOutsideMap(Vector3 relativePos)
    {
        while (this.isActiveAndEnabled)
        {
            transform.position += speed * Time.deltaTime * relativePos.normalized;
            distanceTraveled = (transform.position - Vector3.zero).magnitude;
            if (distanceTraveled >= distanceThreshold)
            {
                distanceTraveled = 0;
                gameObject.SetActive(false);
                break;
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    protected override IEnumerator<float> Move()
    {
        while (true)
        {
            // relative vector from center to object
            Vector3 relativePos = transform.position - Vector3.zero;

            // Align rotation to radius direction vector, in order to always face the center object
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;

            // Update position
            transform.position -= speed * WavesController.Instance.WaveSpeedMultiplier * Time.deltaTime * relativePos.normalized;
            yield return Timing.WaitForOneFrame;
        }
    }
}