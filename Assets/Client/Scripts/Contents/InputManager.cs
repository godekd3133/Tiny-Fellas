using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private bool pressing;


    public Vector3 dragAxis
    {
        get
        {
            if (!pressing) return Vector3.zero;
            Vector3 Axis = dragStartPosition - dragCurrentPosition;
            Axis.z = Axis.y;
            Axis.y = 0;
            if (Axis.magnitude < 0.01f)
            {
                return Vector3.zero;
            }

            Axis.Normalize();

            Debug.Log(Axis);
            return Axis;
        }
    }

    private void Awake()
    {
        instance = this;
        pressing = false;
        dragStartPosition = Vector3.zero;
    }

    void Start()
    {
    }

    void Update()
    {
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Down");
            pressing = true;
            dragStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressing = false;
        }
        if (pressing) dragCurrentPosition = Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID

#endif
    }
}
