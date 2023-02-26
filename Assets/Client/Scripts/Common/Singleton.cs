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

