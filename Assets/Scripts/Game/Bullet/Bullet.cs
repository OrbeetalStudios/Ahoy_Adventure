using MEC;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float speed;
    [SerializeField]
    private Rigidbody rb;
    private readonly float distanceThreshold = 300f;

    void OnEnable()
    {
        var targetObject = GameObject.FindWithTag("Player");
        if (targetObject == null)
        {
            Debug.LogError("the bullet script dosen't find the player target!");
            return;
        }

        target = targetObject.transform;
        transform.position = targetObject.transform.position - targetObject.transform.forward * distance; // position of bullet forward player
        Vector3 perpendicularDirection = Quaternion.Euler(0, 180, 0) * target.forward;//Instantiate the bullet towards the sides of the map
        Timing.RunCoroutine(BulletMovement(perpendicularDirection).CancelWith(gameObject));
    }  
    protected IEnumerator<float> BulletMovement(Vector3 perpendicularDirection)
    {
        Vector3 oldPosition = transform.position;
        float distanceTraveled = 0.0f;

        while (isActiveAndEnabled)
        {
            if (this != null)
            {
                rb.velocity = perpendicularDirection * speed;
                //Direction outside map
                
                distanceTraveled = (transform.position - oldPosition).magnitude;
                if (distanceTraveled >= distanceThreshold)
                {
                    gameObject.SetActive(false);
                    break;
                }
                yield return Timing.WaitForOneFrame;
            }
        }       
    }
}