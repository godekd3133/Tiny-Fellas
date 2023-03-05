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
    [ShowInInspector, ReadOnly] public Minion leaderMinion =>  GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList[0] ;

    public IReadOnlyList<Minion> RecognizedEnemyMinionList
    {
        get;
        private set;
    } = new Minion[1];


    public UnityEvent<Minion> onPostMinionAdded;

    public override void OnNetworkSpawn()
    {
//       if(IsClient) CameraManager.Instance.followingTarget = leaderMinion.transform;
     if(IsServer)  DetectEnemyUpdate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask DetectEnemyUpdate(CancellationToken cancellationToken)
    {
        const float updateInterval = 0.15f;
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
}
