using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Unity.VisualScripting;

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
                        Material[] materials = render.materials;
                        for(int i =0; i<materials.Length; i++){
                            if(materials[i].name.Contains("Chiglia")){
                                materials[i] = highlightMaterial;
                            }
                        }
                        render.materials = materials;
                        // render.material = highlightMaterial;
                        IsHit = true;
                    }
                }

            }
            else
            {
                if (IsHit == true)
                {
                    render = lastHitEnemy.GetComponentInChildren<MeshRenderer>();
                    Material[] materials = render.materials;
                    for(int i =0; i<materials.Length; i++){
                        if(materials[i].name.Contains("barile")){
                            materials[i] = originalMaterial;
                        }
                    }
                    render.materials = materials;
                    // render.material = originalMaterial;
                    lastHitEnemy = null; // Azzera l'ultimo nemico colpito
                    IsHit = false;
                }

            }
            yield return Timing.WaitForOneFrame;
        }
    }
}
