using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class SpawnPoint : MonoBehaviour
{
    private Vector3 newPosition = new Vector3();
    [HideInInspector] public float currAngle;

    [ProButton]
    public void SetPosition(float radius, float angle)
    {
        currAngle = angle;

        newPosition.x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        newPosition.z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        transform.position = newPosition;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3);
    }
}
