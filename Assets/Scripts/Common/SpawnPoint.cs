using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class SpawnPoint : MonoBehaviour
{
    private Vector3 newPosition = new();
    public float currAngle;
    [SerializeField] private float currDistance;

    private void Awake()
    {
        RefreshGui();
    }
    [ProButton]
    public void SetPosition(float radius, float angle)
    {
        newPosition.x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        newPosition.z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        transform.position = newPosition;

        RefreshGui();
    }
    private void UpdateData()
    {
        // signedAngle is between -180 and 180. need to convert to 0 - 360 range
        float signedAngle = Vector3.SignedAngle(transform.position - transform.parent.position, transform.right, Vector3.up);
        currAngle = signedAngle < 0f ? signedAngle + 360f : signedAngle;

        currDistance = Vector3.Distance(transform.position, transform.parent.position);
    }
    [ProButton]
    private void RefreshGui()
    {
        UpdateData();
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3);
    }
}