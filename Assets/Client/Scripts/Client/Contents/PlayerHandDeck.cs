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

    // called from button
    public void Spawn(int targetHandIndex)
    {
        GameSessionInstance.Instance.SpawnMinion_ServerRPC(NetworkManager.Singleton.LocalClientId, minionDataIndexPerButton[targetHandIndex]);
    }

    [ClientRpc]
    public void UpdateHandDeck(int targetHandIndex, int minionDataindexInDeck, ClientRpcParams param = default)
    {
        var playerData =
            GameSessionInstance.Instance.PlayerDataByClientID[param.Send.TargetClientIdsNativeArray.Value[0]];

        var minionData = playerData.MinionDeck[minionDataindexInDeck];
        statThumbnailList[targetHandIndex].sprite = minionData.Stat.MyBattleAbility.Thumbnail;
        minionThumbnailList[targetHandIndex].sprite = minionData.Thumbnail;
        gemCostList[targetHandIndex].text = minionData.Stat.MyBattleAbility[EStatName.GEM_COST].CurrentValue.ToString();
    }
}
