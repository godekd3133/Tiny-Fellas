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

public class GameSessionInstance : NetworkManager
    #if UNITY_SERVER || UNITY_EDITOR
    ,IMinionDeployable
    #endif
{
    private List<PlayerData> playerDataList;
    private Dictionary<string, PlayerData> playerDataByPlayerID ;
    private List<MinionData> minionDeck;
    private List<Minion> minionInstanceList;
    private GameSession gameSession;

    public IReadOnlyList<MinionData> MinionDeck => minionDeck;
    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
    public IReadOnlyList<PlayerData> PlayerDataList => playerDataList;

    public IReadOnlyDictionary<string, PlayerData> PlayerDataByPlayerID
    {
        get
        {
            if (playerDataByPlayerID == null)
            {
                playerDataByPlayerID = new Dictionary<string, PlayerData>();
                foreach (var playerData in playerDataList)
                    playerDataByPlayerID.Add(playerData.PlayerSession.PlayerId, playerData);
            }

            return playerDataByPlayerID;
        }
    }


    public bool AddPlayer(PlayerSession newPlayer)
    {
        return true;
    }

    public bool RemovePlayer(PlayerSession player)
    {
        return true;
    }
}
