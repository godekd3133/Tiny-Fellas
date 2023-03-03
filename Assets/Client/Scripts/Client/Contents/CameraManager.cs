using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CameraManager : MonoWeakSingleton<CameraManager>
{
    public Camera camera;

    public Transform followingTarget;
    public Vector3 followingOffset;


    public void Awake()
    {

    }

    public void Update()
    {
        if (followingTarget != null)
            camera.transform.position = Vector3.Lerp(camera.transform.position, followingTarget.transform.position + followingOffset, Time.deltaTime * 2f);

    }

    [Button("Apply Position")]
    public void Apply(Transform testAnchor, Vector3 offset)
    {
        followingTarget = testAnchor;
        followingOffset = offset;

        camera.transform.position = followingTarget.transform.position + followingOffset;
    }
}
