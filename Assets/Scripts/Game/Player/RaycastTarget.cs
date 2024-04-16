using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class RaycastTarget : MonoBehaviour
{
    private Enemy lastHitEnemy;
    private bool IsHit = false;
    private Renderer render;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material originalMaterial;
    void OnEnable()
    {
        Timing.RunCoroutine(RayCastTarget().CancelWith(gameObject));
    }
    protected IEnumerator<float> RayCastTarget()
    {
        while (isActiveAndEnabled)
        {
            Debug.DrawRay(transform.position, -transform.forward * raycastDistance, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, raycastDistance, raycastMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemyComponent = hit.collider.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        lastHitEnemy = enemyComponent;
                        render = enemyComponent.GetComponentInChildren<MeshRenderer>();
                        render.material = highlightMaterial;
                        IsHit = true;
                    }
                }

            }
            else
            {
                if (IsHit == true)
                {
                    render = lastHitEnemy.GetComponentInChildren<MeshRenderer>();
                    render.material = originalMaterial;
                    lastHitEnemy = null; // Azzera l'ultimo nemico colpito
                    IsHit = false;
                }

            }
            yield return Timing.WaitForOneFrame;
        }
    }
}
