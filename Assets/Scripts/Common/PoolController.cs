using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoSingleton<PoolController>, IPowerUpEvent
{
    [SerializeField] private List<PoolObject> collections;

    private void OnEnable()
    {
        InitializeCollections();
    }
    private void Start()
    {
        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);
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
    public void DeactivateAll()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private List<GameObject> GetCollection(EPoolObjectType id)
    {
        foreach (PoolObject coll in collections)
        {
            if (coll.objID == id)
            {
                return coll.collection;
            }
        }

        // If the specified id is not found, return null
        return null;
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        if (data.Type != EPowerUpType.BulletSize) return;

        List<GameObject> bullets = GetCollection(EPoolObjectType.bullet);
        if (bullets == null) return;

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<Bullet>().SetObjectScale(data.Value);
        }
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        // nothing
    }
}