using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Unity.Netcode;

// empty classs for inherit NetworkManager

public class GameSessionInstance : NetworkBehaviourSingleton<GameSessionInstance>
    #if UNITY_SERVER || UNITY_EDITOR
    ,IMinionDeployable
    #endif
{
    [SerializeField] private bool isLocalTest;
    
    private NetworkVariable<List<PlayerData>> playerDataList;
    private Dictionary<string, PlayerData> playerDataByPlayerID ;
    private List<MinionData> minionDeck;
    private List<Minion> minionInstanceList;
    private GameSession gameSession;

#if UNITY_SERVER || UNITY_EDITOR
    public IReadOnlyList<MinionData> MinionDeck => minionDeck;
    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
#endif
    public IReadOnlyList<PlayerData> PlayerDataList => playerDataList.Value;

    public IReadOnlyDictionary<string, PlayerData> PlayerDataByPlayerID
    {
        get
        {
            if (playerDataByPlayerID == null)
            {
                playerDataByPlayerID = new Dictionary<string, PlayerData>();
                foreach (var playerData in playerDataList.Value)
                    playerDataByPlayerID.Add(playerData.PlayerSession.PlayerId, playerData);
            }

            return playerDataByPlayerID;
        }
    }

    [ServerRpc]
    public bool Connect(string playerSessionID, ulong clientID)
    {
        //TODO : connect to server then server Generating PlayerData with get some data from DB with playerSessionID
        return true;
    }
}
