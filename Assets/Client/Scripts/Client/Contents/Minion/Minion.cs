using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(MinionInstance), typeof(NetworkObject))]
public class Minion : NetworkBehaviour, IIndexContainable
{
    public TroopAdmin troopAdmin;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;

    [SerializeField] Animator animator;

    private int? indexInMinionInstanceList;

    public int? IndexInContainer
    {
        get => indexInMinionInstanceList;
        set
        {
            if (indexInMinionInstanceList == null) return;
            indexInMinionInstanceList = value;
        }
    }

    public UnityEvent<Minion> OnPreAttack { get; private set; }
    public UnityEvent<Minion> OnPostAttack { get; private set; }
    public UnityEvent<Minion> OnPreDamaged { get; private set; }
    public UnityEvent<Minion> OnPostDamaged { get; private set; }

    private MinionState minionState;
    public MinionState MinionState => minionState;

    private MinionInstance stat;
    public MinionInstance Stat => stat;

    public Minion chaseTarget;

    private float lastBattleTime;
    public float LastBattleTIme => lastBattleTime;

    public void OnSpawn()
    {
    }
    private void Awake()
    {
        //        stat.MyBattleAbility.\haviour.SetOwner(this, animator);
        //      stat.MyBattleAbility.PassiveSkill.ApplyEffect(this);
    }

    private async UniTask StatUpdate(CancellationToken cancellationToken)
    {
        while (true)
        {

            await UniTask.NextFrame();
        }
    }

    private async UniTask StateUpdate(CancellationToken cancellationToken)
    {
        minionState = new MinionStateIdle(this);
        minionState.EnterState();
        CancellationTokenSource stateChangeToken = new CancellationTokenSource();
        minionState.UpdateState(CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, stateChangeToken.Token).Token).Forget();

        while (true)
        {
            if (cancellationToken.IsCancellationRequested) break;

            MinionState newState = minionState.CheckTransition();
            if (newState != minionState)
            {
                stateChangeToken.Cancel();
                await UniTask.WaitUntil(() => minionState.enabled == false);
                await minionState.ExitState();
                minionState = newState;

                await minionState.EnterState();
                stateChangeToken = new CancellationTokenSource();
                minionState.UpdateState(CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, stateChangeToken.Token).Token).Forget();
            }
            Debug.Log(minionState);
            await UniTask.NextFrame();
        }
    }

    public override void OnNetworkSpawn()
    {
        Initailize();
    }

    private async UniTask Initailize()
    {
        await UniTask.WaitUntil(waitForSpawningDone);
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        stat = GetComponent<MinionInstance>();
        chaseTarget = null;
        if (NetworkManager.Singleton.IsClient)
        {
            Destroy(agent);
            Destroy(obstacle);
            var ownerID = OwnerClientId;
            var ownerPlayerData = GameSessionInstance.Instance.PlayerDataByClientID[ownerID];
            ownerPlayerData.AddMinionInstance(gameObject);
            gameObject.SetActive(true);
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            agent.speed = 1f;
            lastBattleTime = -1f;
            StatUpdate(this.GetCancellationTokenOnDestroy()).Forget();
            StateUpdate(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    private bool waitForSpawningDone()
    {
        return IsSpawned && gameObject.activeSelf;
    }

    public void Attack(Minion target)
    {
        OnPreAttack?.Invoke(this);
        stat.Attack(target);
        AttackAnimation_ClientRPC();
        if (IsServer) UpdateBattleTime();
        OnPostAttack?.Invoke(this);
    }

    [ClientRpc]
    public void AttackAnimation_ClientRPC()
    {
        stat.MyAttackBehaviour.AttackAnimation();
    }


    public bool TakeDamage(BattleAbilityInstance battleAbility)
    {
        OnPreDamaged?.Invoke(this);
        var flag = stat.TakeDamage(this, battleAbility);

        if (IsServer) UpdateBattleTime();
        OnPostDamaged?.Invoke(this);

        return true;
    }


    public void UpdateBattleTime()
    {
        lastBattleTime = GameSessionInstance.Instance.GameTime.Value;
    }
}
