using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : LinearMotionToTarget
{
    [SerializeField] private int collisionSfxIndex;
    void Start()
    {
        targetPosition = Vector3.zero;
        targetPosition.y = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.gameObject.SetActive(false);
            GameObject effect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.collision_with_barrels);
            PlayVFX(gameObject,effect);
            PlaySFX(collisionSfxIndex);
        }
        else if (other.tag == "Island")
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
