using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    public Map map;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
