using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLayer : MonoBehaviour
{
    internal static HashSet<EnvLayer> Instances = new HashSet<EnvLayer>();

    [System.Serializable]
    public class Settings
    {
        public eLAYER_TYPE Layer;
        public bool IsStopped;

        public Settings(eLAYER_TYPE layer = eLAYER_TYPE.Default, bool stop = false)
        {
            Layer = Layer != eLAYER_TYPE.Default ? Layer : layer;
            IsStopped = stop;
        }
    }
    public Settings Layer = new Settings();

    private void Start() => Init();

    private void OnDisable() => Instances.Remove(this);

    private void OnDestroy() => Instances.Remove(this);

    public virtual void Init(eLAYER_TYPE layer = eLAYER_TYPE.Default, bool stop = false)
    {
        Layer = new Settings(layer, stop);
        Instances.Add(this);
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
        Debug.Log($"{gameObject.name} : Update");
    }

    public virtual void OnLateUpdate()
    {
    }
}
