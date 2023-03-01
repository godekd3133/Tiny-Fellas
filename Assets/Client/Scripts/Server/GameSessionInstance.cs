using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Unity.Netcode;


[RequireComponent(typeof(NetworkObject))]
public class GameSessionInstance : NetworkBehaviourSingleton<GameSessionInstance>
    #if UNITY_SERVER || UNITY_EDITOR
    ,IMinionDeployable
    #endif
{
    [SerializeField] private bool isLocalTest;
    
    private List<PlayerData> playerDataList;
    private Dictionary<ulong, PlayerData> playerDataByClientID ;
    private List<MinionData> minionDeck;
    private List<Minion> minionInstanceList;
    private GameSession gameSession;
    
#if UNITY_SERVER || UNITY_EDITOR
    public IReadOnlyList<MinionData> MinionDeck => minionDeck;
    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
#endif
    public IReadOnlyList<PlayerData> PlayerDataList => playerDataList;

    public IReadOnlyDictionary<ulong, PlayerData> PlayerDataByClientID
    {
        get
        {
            if (playerDataByClientID == null)
            {
                playerDataByClientID = new Dictionary<ulong, PlayerData>();
                foreach (var playerData in playerDataList)
                    playerDataByClientID.Add(playerData.ClientID, playerData);
            }

            return playerDataByClientID;
        }
    }
    

    [ServerRpc]
    public void Connect_ServerRPC(string playerSessionID, ulong clientID)
    {
        if (PlayerDataByClientID.ContainsKey(clientID)) return;
        
        //TODO : connect to server then server Generating PlayerData with get some data from DB with playerSessionID

        var tesstMinionIndexList = new List<int>();
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(1);
        tesstMinionIndexList.Add(2);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(5);

        var testMinionStatIndexList = new List<int>(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);


        var testDeck =
            MinionDataBaseIngame.Instance.GetMinionDeck(tesstMinionIndexList, testMinionStatIndexList);
        var newPlayerData = new PlayerData(testDeck, playerSessionID,clientID);
        newPlayerData.currentGem = 50;
        
        playerDataList.Add(newPlayerData);
    }

    [ClientRpc]
    public void ConnectResponse_ClientRPC()
    {
        
    }

    [ServerRpc]
    public void SpawnMinion_ServerRPC(ulong clientID, int minionDataIndex)
    {
        var playerData = PlayerDataByClientID[clientID];
        var minionData = playerData.MinionDeck[minionDataIndex];

        bool isPurchasable = playerData.currentGem >= minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue;
        if (isPurchasable)
        {
           var newMinion =  Instantiate(minionData.Prefab);
           var newMinionNetworkobject = newMinion.GetComponent<NetworkObject>();
           newMinionNetworkobject.SpawnWithOwnership(clientID);
           
           playerData.AddMinionInstance(newMinion);
        }
    }
}


