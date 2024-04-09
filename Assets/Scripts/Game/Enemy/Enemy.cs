
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : EnemyMovement
{
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.5f;

     void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                GameController.Instance.UpdateLife();
                gameObject.SetActive(false);
                break;
            case "Bullet":
                GameController.Instance.UpdateScore();
                SpawnBox();
                other.gameObject.SetActive(false);//Deactivate Bullet
                gameObject.SetActive(false);
                break;
            case "Island":
                StartPlunder();
                break;
        }        
     }
    private void SpawnBox()
    {
        if (Random.Range(0f, 1f) > spawnChance) return; // if random value is between 0 and spawnChanche, go on and spawn a box

        GameObject box = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.box);
        box.transform.localPosition = this.transform.localPosition;
        box.SetActive(true);
    }
}