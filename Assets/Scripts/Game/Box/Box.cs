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
            GameObject effect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.prop_disappear);
            PlayVFX(gameObject,effect);
        }
    }

    private void PlayVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.position;
        effect.SetActive(true);
    }
}