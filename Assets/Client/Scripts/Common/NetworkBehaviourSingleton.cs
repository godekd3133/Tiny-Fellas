using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkBehaviourSingleton<T> : NetworkBehaviour where T: NetworkBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    public static T Instance => LazyInstance.Value;

    private static T CreateSingleton()
    {
        var precreatedObj = GameObject.FindObjectOfType<T>();
        if (precreatedObj != null) return precreatedObj;
        
        var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}
