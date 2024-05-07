using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : LinearMotionToTarget
{
    [SerializeField] private int boxLostSfxIndex;
    [SerializeField] private int boxCollectedSfxIndex;
    [SerializeField] private int lifeUpSfxIndex;
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
            int i = PowerUpController.Instance.ActivateRandomPowerUp();
            if(i==0){ // index 0 is the one assigned to LifeUp in PowerUpController prefab
                PlaySFX(lifeUpSfxIndex);
            } else {
                PlaySFX(boxCollectedSfxIndex);
            }
            this.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Island"))
        {
            this.gameObject.SetActive(false);
            GameObject effect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.prop_disappear);
            PlayVFX(gameObject,effect);
            PlaySFX(boxLostSfxIndex);
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