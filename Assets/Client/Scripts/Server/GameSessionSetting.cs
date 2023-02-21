

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSessionSetting", menuName = "ScriptableObjects/GameSessionSetting")]
public class GameSessionSetting : ScriptableObject
{
    [SerializeField] private string scenePath;
    [SerializeField] private RangeInt gemBoxCountRange;
    [SerializeField] private List<int> gemCountPerBoxTable;


    public string ScenePath => new (scenePath);
    public RangeInt GemBoxCountRange => gemBoxCountRange;
    public IReadOnlyList<int> GemCountPerBoxTable => gemCountPerBoxTable;
}
