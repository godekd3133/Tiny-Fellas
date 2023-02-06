using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "StatContainer", menuName = "ScriptableObjects/StatContainer", order = 1)]
public class StatContainer : SerializedScriptableObject
{
    [SerializeField] 
    private Dictionary<EStatName, Stat> statMap;

    public ReadOnlyDictionary<EStatName, Stat> StatMap
    {
        get => new (statMap);
    }
}
