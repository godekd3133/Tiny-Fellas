using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class PlayerData : IMinionDeployable
{
    private List<MinionData> minionDeck;
    private List<Minion> minionInstanceList;
    private string playerSessionID;
    private ulong clientID;

    public float currentGem;

    public IReadOnlyList<MinionData> MinionDeck => minionDeck;

    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;

    public string PlayerSessionID => playerSessionID;
    public ulong ClientID => clientID;

    public void AddMinionInstance(GameObject newMinion)
    {
        var minion = newMinion.GetComponent<Minion>();
        minion.IndexInContainer = minionInstanceList.Count;
        minionInstanceList.Add(minion);
    }

    [ClientRpc]
    private void AddMinionInstance()
    {
        
    }

    private PlayerData()
    {
    }

    public PlayerData(List<MinionData> deck, string playerSessionId , ulong clientID)
    {
        minionDeck = deck;
        this.playerSessionID = playerSessionId;
        this.clientID = clientID;
    }
}
