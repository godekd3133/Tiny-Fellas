using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StatContainer", menuName = "ScriptableObjects/StatContainer")]
public class StatContainer : SerializedScriptableObject
{
    [SerializeField]
    private Dictionary<EStatName, Stat> statMap;

    public ReadOnlyDictionary<EStatName, Stat> StatMap => new(statMap);

    public Stat this[EStatName name] => StatMap[name];
}

public class StatContainerInstance
{
    [SerializeField]
    private Dictionary<EStatName, Stat> statMap;

    public ReadOnlyDictionary<EStatName, Stat> StatMap => new(statMap);

    public Stat this[EStatName name] => StatMap[name];

    protected StatContainerInstance()
    {
    }

    public StatContainerInstance(StatContainer origin)
    {
        statMap = new Dictionary<EStatName, Stat>();
        foreach (var pair in origin.StatMap)
            statMap.Add(pair.Key, pair.Value);
    }
}
