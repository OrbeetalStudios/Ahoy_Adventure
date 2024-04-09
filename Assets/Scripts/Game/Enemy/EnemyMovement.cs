using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float enemySpeed;
    [SerializeField]
    private GameObject assaultArea;  
    public int plunderTime;
    public int plunderDefault;
    public bool inPlunder = false;
    private float distanceTraveled;
    private float distanceThreshold = 150f;
    [SerializeField]
    private Renderer render;
    [SerializeField]
    private Material originalMaterial;

    private void OnEnable()
    {
        Timing.RunCoroutine(Move().CancelWith(gameObject));
    }
    private void Awake()
    {
        plunderDefault = plunderTime;   
    }
    protected void StartPlunder()
    {
        StopCoroutine("Move");
        Vector3 relativePos = transform.position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(relativePos,Vector3.up);
        rotation *= Quaternion.Euler(0, 90, 0);
        transform.rotation = rotation;
        assaultArea.SetActive(true);
        inPlunder = true;   
        Timing.RunCoroutine(Plunder().CancelWith(gameObject));
    }
    private  IEnumerator<float> Move()
    {
        while (true)
        {
            // relative vector from center to object
            Vector3 relativePos = transform.position - Vector3.zero;

            // Align rotation to radius direction vector, in order to always face the center object
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;

            // Update position
            transform.position -= enemySpeed * Time.deltaTime * relativePos.normalized;
            yield return Timing.WaitForOneFrame;
        }
    }
    protected IEnumerator<float> Plunder()
    { 
        while (inPlunder==true)
        {
            if (plunderTime <= 0)
            {
                assaultArea.SetActive(false);
                Vector3 relativePos = transform.position - Vector3.zero;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation *= Quaternion.Euler(0, 180, 0);
                transform.rotation = rotation;
                inPlunder = false;
                Timing.RunCoroutine(ReturnOutsideMap(relativePos).CancelWith(gameObject));
            }
            plunderTime--;
            yield return Timing.WaitForSeconds(1f);
        }
       
    }
    public void RestartPlunder()
    {
        Timing.RunCoroutine(Plunder().CancelWith(gameObject));
    }
    protected IEnumerator<float> ReturnOutsideMap(Vector3 relativePos)
    {
        plunderTime = plunderDefault;
        while (this.isActiveAndEnabled)
        {
            transform.position += relativePos.normalized * enemySpeed * Time.deltaTime;
            distanceTraveled = (transform.position - Vector3.zero).magnitude;
            if (distanceTraveled >= distanceThreshold)
            {
                //DeactivateEnemyAtdistanceThreshold
                gameObject.SetActive(false);
                break;
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    private void OnDisable()
    {
        assaultArea.SetActive(false);
        StopAllCoroutines();
        plunderTime = plunderDefault;
        render.material = originalMaterial;
    }
}