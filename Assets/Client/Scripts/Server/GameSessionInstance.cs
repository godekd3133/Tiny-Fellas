using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(NetworkObject))]
public class GameSessionInstance : NetworkBehaviourSingleton<GameSessionInstance>
#if UNITY_SERVER || UNITY_EDITOR
    , IMinionDeployable
#endif
{
    private List<PlayerData> playerDataList = new List<PlayerData>();
    private Dictionary<ulong, PlayerData> playerDataByClientID;
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

    private void Awake()
    {
#if !UNITY_SERVER
        
#endif
    }


    [ServerRpc(RequireOwnership = false)]
    public void Connect_ServerRPC(string playerSessionID, ulong clientID)
    {
        Debug.Log("Connect_ServerRPC");
        if (!IsServer) return;

        if (PlayerDataByClientID.ContainsKey(clientID)) return;
        Debug.Log(string.Format("server accept connection from ID {0}", clientID));
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
        var newPlayerData = new PlayerData(testDeck, playerSessionID, clientID);
        newPlayerData.currentGem = 50;

        playerDataList.Add(newPlayerData);
    }

    [ClientRpc]
    private void BroadCastNewPlayerConnection_ClientRPC(ulong clientID)
    {
        if (playerDataByClientID.ContainsKey(clientID)) return;
        
        var playerData = new PlayerData(null,string.Empty,clientID);
        playerDataList.Add(playerData);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnMinion_ServerRPC(ulong clientID, int minionDataIndex)
    {
        SpawnMinion(clientID,minionDataIndex);
    }

    private void SpawnMinion(ulong clientID, int minionDataIndex, bool forcePurchaseEvenNoGem = false)
    {
        var playerData = PlayerDataByClientID[clientID];
        var minionData = playerData.MinionDeck[minionDataIndex];

        bool isPurchasable =forcePurchaseEvenNoGem || playerData.currentGem >= minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue;
        if (isPurchasable)
        {
            var newMinion = Instantiate(MinionDataBaseIngame.Instance.MinionNetworkPrefabList[minionDataIndex]);
            var newMinionNetworkobject = newMinion.GetComponent<NetworkObject>();
            newMinion.GetComponent<MinionInstanceStat>().AssignOriginStat(minionData.Stat);
            newMinion.GetComponent<AttackBehaviourBase>().SetOwner(newMinion.GetComponent<Minion>());

            // spawned minion's OnNetworkSpawn logic will automatically add itself to player data's minion instance list in client
            newMinionNetworkobject.SpawnWithOwnership(clientID);

            playerData.AddMinionInstance(newMinion);
            playerData.currentGem -= minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue;
        }
    }
}



