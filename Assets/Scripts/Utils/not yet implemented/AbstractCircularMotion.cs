using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCircularMotion : AbstractMotionToTarget
{
    [SerializeField]
    protected bool clockwiseMotion = false;

    protected float angle = 0.0f;

    public void InvertDirection() => clockwiseMotion = !clockwiseMotion; 
}
