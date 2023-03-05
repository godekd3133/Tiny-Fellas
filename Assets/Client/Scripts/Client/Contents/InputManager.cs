using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InputManager : NetworkBehaviour
{
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private bool pressing;

    private Vector3 previousDrageAxis = Vector3.zero;

    private NetworkVariable<Vector3> latestDragAxis = new NetworkVariable<Vector3>(default,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public Vector3 DragAxis => latestDragAxis.Value;


    private Vector3 DragAxis_Client
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

            return Axis;
        }
    }

    public override void OnNetworkSpawn()
    {
        latestDragAxis.OnValueChanged = (previous, current) =>
        {
            latestDragAxis.Value =  current;
        };
    }

    private void Awake()
    {
        pressing = false;
        dragStartPosition = Vector3.zero;
    }

    void Update()
    {
        if (IsServer)
        {
            #if UNITY_EDITOR
            Debug.Log(latestDragAxis);
            #endif
            return;
        }
        
        latestDragAxis.SetDirty(false);
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            pressing = true;
            dragStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressing = false;
        }
        if (pressing) dragCurrentPosition = Input.mousePosition;

        var currentDragAxis = DragAxis_Client;
        if (currentDragAxis != previousDrageAxis)
        {
            latestDragAxis.Value = DragAxis_Client;
            previousDrageAxis = currentDragAxis;
        }
#elif UNITY_IOS || UNITY_ANDROID

#endif
    }
}
