using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "StatContainer", menuName = "ScriptableObjects/StatContainer")]
public class StatContainer : SerializedScriptableObject
{
    [SerializeField] 
    private Dictionary<EStatName, Stat> statMap;

    public ReadOnlyDictionary<EStatName, Stat> StatMap => new (statMap);
    
    public Stat this[EStatName name] => StatMap[name];
}
