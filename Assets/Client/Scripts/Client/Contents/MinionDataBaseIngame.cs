using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class MinionDataBaseIngame : MonoWeakSingleton<MinionDataBaseIngame>
{
    [SerializeField] private MinionDataBase minionDataBase;
    [SerializeField] private MinionStatDataBase minionStatDataBase;
   [SerializeField] private List<GameObject> minionNetworkPrefabList = new List<GameObject>();

    private NetworkManager networkManager;

    public IReadOnlyList<GameObject> MinionNetworkPrefabList => minionNetworkPrefabList;
    public IReadOnlyDictionary<int, MinionStat> MinionStatDataByIndex => minionStatDataBase.DataByInex;
    public IReadOnlyDictionary<int, MinionData> MinionDataByIndex => minionDataBase.DataByInex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Add minion prefab to NetworkPrefab list")]
    private void UpdateNetworkMinionPrefabList()
    {
        networkManager = NetworkManagerInstance.Instance;
        
        foreach (var minionData in minionDataBase.MinionDatas)
            minionNetworkPrefabList.Add(minionData.Prefab);
    }

    public List<MinionData> GetMinionDeck(List<int> minionIndexList, List<int> statIndexList)
    {
        var deck = new List<MinionData>();
        for (int i = 0; i < minionIndexList.Count; i++)
            deck.Add(GetMinionDataInstance(minionIndexList[i], statIndexList[i]));

        return deck;
    }
    
    public List<MinionData> GetMinionDeck(int[]minionIndexList, int[]statIndexList)
    {
        var deck = new List<MinionData>();
        for (int i = 0; i < minionIndexList.Length; i++)
            deck.Add(GetMinionDataInstance(minionIndexList[i], statIndexList[i]));

        return deck;
    }

    public MinionData GetMinionDataInstance(int minionIndex, int statIndex)
    {
        var originData = minionDataBase.DataByInex[minionIndex];
        var originStat = minionStatDataBase.DataByInex[statIndex];
        return new MinionData(minionNetworkPrefabList[minionIndex],originData.Thumbnail,originStat,originStat.IndexInContainer);
    }
}

