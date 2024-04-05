using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float speed;
    protected float currentSpeed;

   

    protected void Update()
    {
        currentSpeed = speed;//For Prototype changes in-game
    }

    [HideInInspector]
    protected bool clockwiseMotion = false;

    protected float angle = 0.0f;

    public void InvertDirection() => clockwiseMotion = !clockwiseMotion;
}
