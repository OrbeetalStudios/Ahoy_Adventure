using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    [SerializeField] private bool bDontDestroyOnLoad = true;

    protected void Awake()
    {
        if (instance != null)
        {
            //Debug.LogError("Instance of this singleton " + (T)this + " already exists, deleting!");
            Destroy(gameObject);
        }
        else
        {
            if (bDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            instance = (T)this;
        }
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
}