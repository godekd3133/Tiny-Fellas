using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(NetworkObject))]
public class GameSessionInstance : NetworkBehaviourSingleton<GameSessionInstance>
#if UNITY_SERVER || UNITY_EDITOR
    , IMinionDeployable
#endif
{
    [SerializeField] private PlayerHandDeck handDeck;
    #if UNITY_EDITOR
    [SerializeField]
    #endif
    private List<PlayerData> playerDataList = new List<PlayerData>();
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<MinionData> minionDeck;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<Minion> minionInstanceList;
    private Dictionary<ulong, PlayerData> playerDataByClientID = new Dictionary<ulong, PlayerData>();
    private GameSession gameSession;

#if UNITY_SERVER || UNITY_EDITOR
    public IReadOnlyList<MinionData> MinionDeck => minionDeck;
    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
#endif
    public IReadOnlyList<PlayerData> PlayerDataList => playerDataList;

    public IReadOnlyDictionary<ulong, PlayerData> PlayerDataByClientID
    {
        get => playerDataByClientID;
    }

    private void Awake()
    {
        
#if !UNITY_SERVER

#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Connect_ServerRPC("", NetworkManager.LocalClientId);
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
        playerDataByClientID.Add(clientID,newPlayerData);
        BroadCastNewPlayerConnection_ClientRPC(clientID, tesstMinionIndexList.ToArray(), testMinionStatIndexList.ToArray());

        var handDeckIndices = new int[4];
        for (int i = 0; i < handDeckIndices.Length; i++)
            handDeckIndices[i] = Random.Range(0, newPlayerData.MinionDeck.Count);

        var RPCParam = new ClientRpcParams();
        RPCParam.Send = new ClientRpcSendParams()
        {
            TargetClientIds = new ulong[] { clientID }
        };
        handDeck.SetHandDeck_ClientRPC(handDeckIndices,RPCParam);
        
        var playerObject = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }

    [ClientRpc]
    public void BroadCastNewPlayerConnection_ClientRPC(ulong clientID, int[] minionAssetIndexArr, int[] battalAbilityAssetIndexPerMinion)
    {
        if (playerDataByClientID.ContainsKey(clientID)) return;
        
        var testDeck =
            MinionDataBaseIngame.Instance.GetMinionDeck(minionAssetIndexArr, battalAbilityAssetIndexPerMinion);
        var playerData = new PlayerData(testDeck,string.Empty,clientID);
        playerDataList.Add(playerData);
        playerDataByClientID.Add( clientID, playerData);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMinion_ServerRPC(ulong clientID, int minionDataIndex, int targetHandDeckIndex)
    {
       var spawned= SpawnMinion(clientID,minionDataIndex);
       if (spawned)
       {
           var playerData = playerDataByClientID[clientID];
           var randomMinionIndex = Random.Range(0, playerData.MinionDeck.Count);
           var RPCParam = new ClientRpcParams();
           RPCParam.Send = new ClientRpcSendParams
           {
               TargetClientIds = new ulong[] { clientID }
           };
           handDeck.UpdateHandDeck_ClientRPC(targetHandDeckIndex, randomMinionIndex, RPCParam);
       }
    }

    private bool SpawnMinion(ulong clientID, int minionDataIndex, bool forcePurchaseEvenNoGem = false)
    {
        var playerData = PlayerDataByClientID[clientID];
        var minionData = playerData.MinionDeck[minionDataIndex];

        bool isPurchasable =forcePurchaseEvenNoGem || playerData.currentGem >= minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue;
        if (isPurchasable)
        {
            var newMinion = Instantiate(MinionDataBaseIngame.Instance.MinionNetworkPrefabList[minionDataIndex]);
            var newMinionNetworkobject = newMinion.GetComponent<NetworkObject>();
            newMinion.GetComponent<MinionInstance>().AssignOriginStat(minionData.Stat);
            newMinion.GetComponent<AttackBehaviourBase>().SetOwner(newMinion.GetComponent<Minion>());

            // spawned minion's OnNetworkSpawn logic will automatically add itself to player data's minion instance list in client
            newMinionNetworkobject.SpawnWithOwnership(clientID);

            playerData.AddMinionInstance(newMinion);
            playerData.currentGem -= minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue;
        }

        return isPurchasable;
    }
}



