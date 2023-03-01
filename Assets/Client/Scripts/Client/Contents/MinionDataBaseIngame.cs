using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MinionDataBaseIngame : MonoWeakSingleton<MinionDataBaseIngame>
{
    [SerializeField] private MinionDataBase minionDataBase;
    [SerializeField] private MinionStatDataBase minionStatDataBase;
   [SerializeField] private List<GameObject> minionNetworkPrefabList = new List<GameObject>();

    private NetworkManager networkManager;

    public IReadOnlyList<GameObject> MinionNetworkPrefabList => minionNetworkPrefabList;

    private void Awake()
    {
    }

    [ContextMenu("Add minion prefab to NetworkPrefab list")]
    private void UpdateNetworkMinionPrefabList()
    {
        networkManager = NetworkManagerInstance.Instance;
        
        foreach (var minionData in minionDataBase.MinionDatas)
        {
            var newPrefab = Instantiate(minionData.Prefab);
            newPrefab.AddComponent<NetworkObject>();
            newPrefab.AddComponent<MinionInstanceStat>();
            networkManager.AddNetworkPrefab(newPrefab);
            minionNetworkPrefabList.Add(newPrefab);
            
            newPrefab.gameObject.SetActive(false);
        }
    }

#if UNITY_SERVER || UNITY_EDITOR
    public List<MinionData> GetMinionDeck(List<int> minionIndexList, List<int> statIndexList)
    {
        var deck = new List<MinionData>();
        for (int i = 0; i < minionIndexList.Count; i++)
            deck.Add(GetMinionDataInstance(minionIndexList[i], statIndexList[i]));

        return deck;
    }

    public MinionData GetMinionDataInstance(int minionIndex, int statIndex)
    {
        return new MinionData(minionNetworkPrefabList[minionIndex],minionDataBase.DataByInex[minionIndex], minionStatDataBase.DataByInex[statIndex]);
    }
    #endif
}

