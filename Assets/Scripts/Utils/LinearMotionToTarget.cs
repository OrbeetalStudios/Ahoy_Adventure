using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LinearMotionToTarget : AbstractMotionToTarget
{
    protected void Start()
    {
        currentSpeed = speed;
    }
    private void OnEnable()
    {
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
