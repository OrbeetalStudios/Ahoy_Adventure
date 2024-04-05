using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class PoolController : MonoSingleton<PoolController>
{
    [SerializeField] private List<PoolObject> collections;
    [SerializeField] private float spawnInterval=30f;
    
    private void Start()
    {
        InitializeCollections();

        Timing.RunCoroutine(SpawnEnemy().CancelWith(gameObject)); 
    }
    private void InitializeCollections()
    {
        foreach (PoolObject coll in collections)
        {
            coll.collection = new List<GameObject>();

            for (int i = 0; i < coll.numberOfObjects; i++)
            {
                GameObject obj = Instantiate(coll.prefab, this.transform);
                obj.SetActive(false);
                coll.collection.Add(obj);
            }
        }
    }
    public GameObject GetObjectFromCollection(EPoolObjectType id)
    {
        foreach (PoolObject coll in collections)
        {
            if (coll.objID == id)
            {
                foreach (GameObject obj in coll.collection)
                {
                    if (!obj.activeSelf)
                    {
                        return obj;
                    }
                }

                // If all objects in the pool are in use, expand the pool
                GameObject newObj = Instantiate(coll.prefab);
                newObj.SetActive(false);
                coll.collection.Add(newObj);
                return newObj;
            }
        }

        // If the specified id is not found, return null
        return null;
    }
    public void SpawnBullet()
    {
        GameObject bullet = GetObjectFromCollection(EPoolObjectType.bullet);
        bullet.SetActive(true);
    }
    protected IEnumerator<float> SpawnEnemy()
    {
        while (true)
        {
            GameObject enemyShip = GetObjectFromCollection(EPoolObjectType.enemy_default);
            enemyShip.SetActive(true);

            yield return Timing.WaitForSeconds(spawnInterval);
        }
    }
}