using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
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

    public IReadOnlyList<Minion> RecognizedEnemyMinionList
    {
        get;
        private set;
    } = new Minion[1];


    public UnityEvent<Minion> onPostMinionAdded;

    public override void OnNetworkSpawn()
    {
       minionList = GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList;
       if(IsClient) CameraManager.Instance.followingTarget = leaderMinion.transform;
       DetectEnemyUpdate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask DetectEnemyUpdate(CancellationToken cancellationToken)
    {
        const float updateInterval = 0.15f;
        await UniTask.WaitUntil(waitForFirstMinionSpawn);
        while (true)
        {
            if (cancellationToken.IsCancellationRequested) break;

            RecognizedEnemyMinionList = PerceptionUtility.GetPerceptedMinionList(leaderMinion, OwnerClientId);
            await UniTask.Delay(TimeSpan.FromSeconds(updateInterval),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update,
                cancellationToken);

        }
    }

    private bool waitForFirstMinionSpawn()
    {
        return minionList.Count > 0;
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
