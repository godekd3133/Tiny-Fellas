using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instace
    {
        get
        {
            if (instance == null) new T();
            return instance;
        }
    }
}

public class SingletonPerScene<T> where T : new()
{
    private static Dictionary<Scene,T> instanceMap = new Dictionary<Scene, T>();

    public static T GetInstanceOfScene(Scene scene)
    {
        if(instanceMap.ContainsKey(scene) == false ) instanceMap.Add(scene, new T());
        else if (instanceMap[scene] == null) instanceMap[scene] = new T();
        return instanceMap[scene];
    }
    
    public static T GetInstanceOfScene(GameObject obj)
    {
        var scene = obj.scene;
        return GetInstanceOfScene(scene);
    }
}
