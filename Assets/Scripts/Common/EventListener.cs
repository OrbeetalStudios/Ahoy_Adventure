using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoSingleton<EventListener>
{
    public List<GameObject> listeners = new List<GameObject>();

    public void AddListener(GameObject go)
    {
        // Don't add if already there
        if (!listeners.Contains(go))
        {
            listeners.Add(go);
        }
    }
}