using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionInfo : ScriptableObject
{
    [SerializeField] private string minionName;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private ulong uniqueID;
}
