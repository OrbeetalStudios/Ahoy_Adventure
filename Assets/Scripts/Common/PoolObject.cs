using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolObject", menuName = "Custom/PoolObject")]
public class PoolObject : ScriptableObject
{
    public EPoolObjectType objID;
    public GameObject prefab;
    public int numberOfObjects;
    [HideInInspector] public List<GameObject> collection;
}
