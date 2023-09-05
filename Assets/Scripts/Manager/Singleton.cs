using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (ReferenceEquals(instance, null))
            {
                instance = FindObjectOfType<T>();
                if (ReferenceEquals(instance, null))
                {
                    var gameObject = new GameObject(nameof(T));
                    instance = gameObject.AddComponent<T>();
                }
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        OnAwake();
    }

    protected virtual void OnAwake() { }
}
