using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerData : IMinionDeployable
{
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<MinionData> minionDeck = new List<MinionData>();

    private int[] minionAssetIndexArrInDeck;
    private int[] battleAbilityIndexArrInDeck;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<Minion> minionInstanceList = new List<Minion>();
#if UNITY_EDITOR
    [SerializeField]
#endif
    private string playerSessionID;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private ulong clientID;
    private bool isBot;

    public float currentGem;

    public IReadOnlyList<MinionData> MinionDeck => minionDeck;

    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
    public int[] MinionAssetIndexArrInDeck => minionAssetIndexArrInDeck;
    public int[] BattleAbilityArrIndeck => battleAbilityIndexArrInDeck;

    public string PlayerSessionID => playerSessionID;
    public ulong ClientID => clientID;
    public bool IsBot => isBot;

    public UnityEvent<Minion> OnPostMinionAdded;

    public void AddMinionInstance(GameObject newMinion)
    {
        var minion = newMinion.GetComponent<Minion>();
        minion.IndexInContainer = minionInstanceList.Count;
        minionInstanceList.Add(minion);
        OnPostMinionAdded?.Invoke(minion);
    }

    [ClientRpc]
    private void AddMinionInstance()
    {

    }

    private PlayerData()
    {
    }

    public PlayerData(string playerSessionId, ulong clientID, int[] minionAssetIndexArrInDeck , int[] battleAbilityIndexArrInDeck,bool isBot)
    {
        minionDeck = MinionDataBaseIngame.Instance.GetMinionDeck(minionAssetIndexArrInDeck, battleAbilityIndexArrInDeck);
        this.playerSessionID = playerSessionId;
        this.clientID = clientID;
        this.isBot = isBot;
        this.minionAssetIndexArrInDeck = minionAssetIndexArrInDeck;
        this.battleAbilityIndexArrInDeck = battleAbilityIndexArrInDeck;
        
        

    }
}
