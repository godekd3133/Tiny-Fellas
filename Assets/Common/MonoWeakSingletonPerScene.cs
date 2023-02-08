using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MonoWeakSingletonPerScene<T> : MonoBehaviour where T : MonoBehaviour
{
    private static Dictionary<Scene,T> instanceMap = new Dictionary<Scene, T>();

    public static T GetInstanceOfScene(Scene scene)
    {
        if(instanceMap.ContainsKey(scene) == false) instanceMap.Add(scene, CreateSingleton());
        return instanceMap[scene];
    }
    
    public static T GetInstanceOfScene(GameObject obj)
    {
        var scene = obj.scene;
        return GetInstanceOfScene(scene);
    }

    private static T CreateSingleton()
    {
        var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}