using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public abstract class AbstractMotionToTarget : MonoBehaviour
{
    protected Vector3 targetPosition; // target point to reach
    [SerializeField, Range(0f, 20f)] protected float speed;
    [SerializeField] protected bool MultiplierAllowed;
    protected CoroutineHandle moveToTargetHandle;

    protected abstract IEnumerator<float> Move();
}
