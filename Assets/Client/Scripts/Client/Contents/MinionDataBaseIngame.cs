using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MinionDataBaseIngame : MonoWeakSingleton<MinionDataBaseIngame>
{
    [SerializeField] private MinionDataBase minionDataBase;
    [SerializeField] private MinionStatDataBase minionStatDataBase;

    private NetworkManager networkManager;
    private List<GameObject> minionPrefabListForServer = new List<GameObject>();
    
    private void Awake()
    {
        #if UNITY_SERVER || UNITY_EDITOR
        networkManager = NetworkManager.Singleton;

        foreach (var minionData in minionDataBase.MinionDatas)
        {
            var newPrefab = Instantiate(minionData.Prefab);
            newPrefab.AddComponent<NetworkBehaviour>();
            newPrefab.AddComponent<NetworkObject>();
            newPrefab.AddComponent<MinionInstanceStat>();
            networkManager.AddNetworkPrefab(newPrefab);
            minionPrefabListForServer.Add(newPrefab);
        }
        #endif
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
        return new MinionData(minionPrefabListForServer[minionIndex],minionDataBase.DataByInex[minionIndex], minionStatDataBase.DataByInex[statIndex]);
    }
    #endif
}
