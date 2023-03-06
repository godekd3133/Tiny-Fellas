using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(NetworkObject))]
public class TroopAdmin : NetworkBehaviour
{
    [ShowInInspector, ReadOnly] public Minion leaderMinion =>  GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList[0] ;
    [SerializeField] private float addingGemPerSec = 10f;
    
    private NetworkVariable<float> currenetGem = new NetworkVariable<float>(0f,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public IReadOnlyList<Minion> RecognizedEnemyMinionList
    {
        get;
        private set;
    } = new Minion[1];

    public float CurrentGem
    {
        get => currenetGem.Value;
        set => currenetGem.Value = value;
    }


    public UnityEvent<Minion> onPostMinionAdded;

    public override void OnNetworkSpawn()
    {
        Initailize();
    }
    
    private async UniTask Initailize()
    {
        await UniTask.WaitUntil(waitForSpawningDone);
        if (IsServer)
        {
            DetectEnemyUpdate(this.GetCancellationTokenOnDestroy()).Forget();
            UpdateGem().Forget();
        }
    }
    

    private bool waitForSpawningDone()
    {
        return IsSpawned && gameObject.activeSelf;
    }
    
    private async UniTask UpdateGem()
    {
        while (true)
        {
            currenetGem.Value += addingGemPerSec;
            await UniTask.Delay(1000);
        }
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
        return GameSessionInstance.Instance.PlayerDataByClientID[OwnerClientId].MinionInstanceList.Count > 0;
    }
}
