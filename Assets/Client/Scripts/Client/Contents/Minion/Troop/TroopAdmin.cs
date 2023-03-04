using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(NetworkObject))]
public class TroopAdmin : NetworkBehaviour
{
    [ShowInInspector, ReadOnly] private IReadOnlyList<Minion> minionList;
    [ShowInInspector, ReadOnly] public Minion leaderMinion { get; private set; }


    public UnityEvent<Minion> onPostMinionAdded;

    public override void OnNetworkSpawn()
    {
        minionList = GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList;
    }

    void Start()
    {
        if(IsClient) CameraManager.Instance.followingTarget = leaderMinion.transform;
    }

    private void Update()
    {
    }

    /// <summary>
    /// 리더 미니언 갱신
    /// </summary>
    /// <returns> Selected leader minion</returns>
    public Minion UpdateLeaderMinion()
    {
        leaderMinion = minionList.First();
        return leaderMinion;
    }
}
