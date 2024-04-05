using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : LinearMotionToTarget
{
    void Start()
    {
        GameObject island;
        if (island = GameObject.FindWithTag("Island"))
        {
            targetPosition = island.transform.position;
        }

        base.Start();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.Instance.UpdateLife();
            this.gameObject.SetActive(false);
        }
        else if (other.tag == "Island")
        {
            this.gameObject.SetActive(false);
        }
    }
}
