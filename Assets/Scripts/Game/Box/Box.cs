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
        if (other.tag == "Player")
        {
            //manage power up

            this.gameObject.SetActive(false);
        }
        else if (other.tag == "Island")
        {
            this.gameObject.SetActive(false);
        }
    }
}
