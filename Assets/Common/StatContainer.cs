using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "StatContainer", menuName = "ScriptableObjects/StatContainer")]
public class StatContainer : SerializedScriptableObject
{
    [SerializeField] 
    private Dictionary<EStatName, Stat> statMap;

    public ReadOnlyDictionary<EStatName, Stat> StatMap => new (statMap);
    
    public Stat this[EStatName name] => StatMap[name];

    protected StatContainer()
    {
    }

    public StatContainer(StatContainer origin)
    {
        statMap = new Dictionary<EStatName, Stat>();
        foreach (var pair in origin.statMap)
            statMap.Add(pair.Key,pair.Value);
    }
}
