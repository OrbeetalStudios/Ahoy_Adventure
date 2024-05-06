using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    protected List<GameObject> models = new();

    protected void Start()
    {
        foreach (Transform child in transform)
        {
            models.Add(child.gameObject);
        }
    }
}