using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LinearMotionToTarget : AbstractMotionToTarget
{
    protected void Start()
    {
        targetPosition = Vector3.zero;
        currentSpeed = speed;
        moveToTargetHandle = Timing.RunCoroutine(Move().CancelWith(gameObject));
    }
    protected override IEnumerator<float> Move()
    {
        while (true)
        {
            // relative vector from this to target
            Vector3 relativePos = targetPosition - transform.position;

            // Update position
            transform.position += currentSpeed * Time.deltaTime * relativePos.normalized;
            
            yield return Timing.WaitForOneFrame;
        }
    }
}
