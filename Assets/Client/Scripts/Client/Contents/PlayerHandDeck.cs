using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkObject))]
public class PlayerHandDeck : NetworkBehaviourSingleton<PlayerHandDeck>
{
    [SerializeField] private List<Image> statThumbnailList;

    [SerializeField] private List<Image> minionThumbnailList;

    [SerializeField] private List<TextMeshProUGUI> gemCostList;
    [SerializeField] private List<Button> buttons;

    private int[] minionDataIndexPerButton = { -1, -1, -1, -1 };

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) Spawn(0);
        if (Input.GetKeyDown(KeyCode.Keypad1)) Spawn(1);
        if (Input.GetKeyDown(KeyCode.Keypad2)) Spawn(2);
        if (Input.GetKeyDown(KeyCode.Keypad3)) Spawn(3);
    }
#endif
    // called from button
    public void Spawn(int targetHandIndex)
    {
        GameSessionInstance.Instance.SpawnMinion_ServerRPC(NetworkManager.Singleton.LocalClientId, minionDataIndexPerButton[targetHandIndex], targetHandIndex);
    }

    [ClientRpc]
    public void UpdateHandDeck_ClientRPC(int targetHandIndex, int minionDataindexInDeck, ClientRpcParams param = default)
    {
        #if UNITY_EDITOR
        Debug.Log("hand deck updated at hand index "+targetHandIndex);
        #endif
        
        var playerData =
            GameSessionInstance.Instance.PlayerDataByClientID[param.Send.TargetClientIds[0]];

        var minionData = playerData.MinionDeck[minionDataindexInDeck];
        statThumbnailList[targetHandIndex].sprite = minionData.Stat.MyBattleAbility.Thumbnail;
        minionThumbnailList[targetHandIndex].sprite = minionData.Thumbnail;
        minionDataIndexPerButton[targetHandIndex] = minionDataindexInDeck;
        gemCostList[targetHandIndex].text = minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue.ToString();
    }
    
    [ClientRpc]
    public void SetHandDeck_ClientRPC(int[] minionDataindices, ClientRpcParams param = default)
    {
#if UNITY_EDITOR
        Debug.Log("hand deck initialized at client");
        #endif
        var playerData =
            GameSessionInstance.Instance.PlayerDataByClientID[NetworkManager.Singleton.LocalClientId];

        for (int i = 0; i < minionDataindices.Length; i++)
        {
            var minionData = playerData.MinionDeck[minionDataindices[i]];
            statThumbnailList[i].sprite = minionData.Stat.MyBattleAbility.Thumbnail;
            minionThumbnailList[i].sprite = minionData.Thumbnail;
            minionDataIndexPerButton[i] = minionDataindices[i];
            gemCostList[i].text =
                minionData.Stat[EStatName.GEM_COST].CurrentValue.ToString();
        }
    }
}
