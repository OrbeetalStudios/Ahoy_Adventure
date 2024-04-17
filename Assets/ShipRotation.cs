using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    public GameObject ship;
    public Transform[] positionPoint;
    [Range(0f, 1f)] public float value;

    private void Update()
    {
        iTween.PutOnPath(ship,positionPoint,value);
    }
    private void OnDrawGizmos()
    {
        iTween.DrawPath(positionPoint);
    }
}
