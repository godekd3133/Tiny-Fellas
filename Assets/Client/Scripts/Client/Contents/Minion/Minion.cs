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
    public float moveSpeed;
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

    public UnityEvent<Minion> beforeAttack { get; private set; }
    public UnityEvent<Minion> afterAttack { get; private set; }
    public UnityEvent<Minion> befroeDamaged { get; private set; }
    public UnityEvent<Minion> afterDamaged { get; private set; }

    private MinionState minionState;

    [HideInInspector] public UnityEvent onStatChanged;
    private MinionInstance stat;

    public MinionInstance Stat => stat;
    public Minion chaseTarget;

    private void Awake()
    {
        //        stat.MyBattleAbility.AttackBehaviour.SetOwner(this, animator);
        //      stat.MyBattleAbility.PassiveSkill.ApplyEffect(this);
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
                minionState.ExitState();
                minionState = newState;

                minionState.EnterState();
                stateChangeToken = new CancellationTokenSource();
                minionState.UpdateState(CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, stateChangeToken.Token).Token).Forget();
            }

            await UniTask.NextFrame();
        }
    }


    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        if (IsClient)
        {
            var ownerID = OwnerClientId;
            var ownerPlayerData = GameSessionInstance.Instance.PlayerDataByClientID[ownerID];
            ownerPlayerData.AddMinionInstance(gameObject);
        }
        else if(IsServer) StateUpdate(this.GetCancellationTokenOnDestroy()).Forget();
     
    }

    public void Attack(Minion target)
    {
        beforeAttack.Invoke(this);
        stat.Attack(target);
        afterAttack.Invoke(this);
    }

    public bool TakdeDamage()
    {
        befroeDamaged.Invoke(this);
        var flag = stat.TakeDamage(this, stat.MyBattleAbility);
        afterDamaged.Invoke(this);

        return true;
    }
}
