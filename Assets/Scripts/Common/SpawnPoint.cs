using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class SpawnPoint : MonoBehaviour
{
    private Vector3 newPosition = new Vector3();
    [HideInInspector] public float currAngle;

    private void Awake()
    {
        // signedAngle is between -180 and 180. need to convert to 0 - 360 range
        float signedAngle = Vector3.SignedAngle(transform.position - transform.parent.position, transform.right, Vector3.up);
        currAngle = signedAngle < 0f ? signedAngle + 360f : signedAngle;
    }
    [ProButton]
    public void SetPosition(float radius, float angle)
    {
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
