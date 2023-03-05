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

    public IReadOnlyList<Minion> Minions => GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList;

    [ShowInInspector, ReadOnly] public Minion leaderMinion => Minions[0];
    public IReadOnlyList<Minion> RecognizedEnemyMinionList
    {
        get;
        private set;
    } = new Minion[1];

    public override void OnNetworkSpawn()
    {
        //       if(IsClient) CameraManager.Instance.followingTarget = leaderMinion.transform;
        if (IsServer) DetectEnemyUpdate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask DetectEnemyUpdate(CancellationToken cancellationToken)
    {
        const float updateInterval = 0.15f;
        await UniTask.WaitUntil(CheckFirstMinionSpawn);
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

    public int LivingMinionCount() => Minions.Count((Minion minion) => minion.MinionState.GetType() != typeof(MinionStateDead));

    public bool IsInBattle() => Minions.Any((Minion minion) => GameSessionInstance.Instance.GameTime.Value - minion.LastBattleTIme < 10f);

    private bool CheckFirstMinionSpawn()
    {
        return GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList.Count > 0;
    }
}
