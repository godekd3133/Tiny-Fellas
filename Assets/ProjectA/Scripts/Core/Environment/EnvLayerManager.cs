    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLayerManager : MonoBehaviour
{
    public static EnvLayerManager Current { get; private set; }

    private void OnEnable() => Current = this;

    private void FixedUpdate()
    {
        var a = EnvLayer.Instances;
        foreach (var iter in a)
            if (!iter.Layer.IsStopped)
            {
                iter.OnFixedUpdate();
            }
    }

    private void Update()
    {
        var a = EnvLayer.Instances;
        foreach (var iter in a)
            if (!iter.Layer.IsStopped)
            {
                iter.OnUpdate();
            }
    }

    private void LateUpdate()
    {
        var a = EnvLayer.Instances;
        foreach (var iter in a)
            if (!iter.Layer.IsStopped)
            {
                iter.OnLateUpdate();
            }
    }
}
