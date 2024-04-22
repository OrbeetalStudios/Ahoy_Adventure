using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : LinearMotionToTarget
{
    void Start()
    {
        targetPosition = Island.Instance.transform.position;
        targetPosition.y = 0f;

        base.Start();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //manage power up
            PowerUpController.Instance.ActivatePowerUp();

            this.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Island"))
        {
            this.gameObject.SetActive(false);
        }
    }
}