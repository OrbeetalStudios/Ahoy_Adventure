using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : LinearMotionToTarget
{
    [SerializeField] private int sfxIndex;
    void Start()
    {
        targetPosition = Vector3.zero;
        targetPosition.y = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //manage power up
            PowerUpController.Instance.ActivateRandomPowerUp();

            this.gameObject.SetActive(false);
            PlaySFX(sfxIndex);
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

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}