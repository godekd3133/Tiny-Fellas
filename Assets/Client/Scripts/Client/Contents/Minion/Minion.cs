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

[RequireComponent(typeof(MinionInstanceStat), typeof(NetworkObject))]
public class Minion : NetworkBehaviour, IIndexContainable
{
    public PlayerData ownerPlayer;
    public float moveSpeed;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public List<Minion> recognizedEnemies;
    public TroopAdmin troopAdmin;

    public bool isLeader => troopAdmin.leaderMinion == this;
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
    private MinionInstanceStat stat;

    public MinionInstanceStat Stat => stat;
    public Minion chaseTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        obstacle = GetComponentInChildren<NavMeshObstacle>();
        //        stat.MyBattleAbility.AttackBehaviour.SetOwner(this, animator);
        //      stat.MyBattleAbility.PassiveSkill.ApplyEffect(this);
        recognizedEnemies = new List<Minion>();

    }

    private void Start()
    {
        StateUpdate(this.GetCancellationTokenOnDestroy()).Forget();
        DetectEnemyUpdate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask DetectEnemyUpdate(CancellationToken cancellationToken)
    {
        const float updateInterval = 0.15f;
        while (true)
        {
            if (cancellationToken.IsCancellationRequested) break;

            recognizedEnemies = PerceptionUtility.GetPerceptedMinionListLocalClient(this);
            await UniTask.Delay(TimeSpan.FromSeconds(updateInterval),
                                DelayType.DeltaTime,
                                PlayerLoopTiming.Update,
                                cancellationToken);

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
                minionState.ExitState();
                minionState = newState;

                minionState.EnterState();
                stateChangeToken = new CancellationTokenSource();
                minionState.UpdateState(CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, stateChangeToken.Token).Token).Forget();
            }

            await UniTask.NextFrame();
        }

    }


    public override void OnNetworkSpawn()
    {
        if (IsServer) return;

        var ownerID = ownerPlayer.ClientID;
        var ownerPlayerData = GameSessionInstance.Instance.PlayerDataByClientID[ownerID];
        ownerPlayerData.AddMinionInstance(gameObject);
        GetComponent<AttackBehaviourBase>().SetOwner(this, animator);
    }

    public void Attack()
    {
        beforeAttack.Invoke(this);
        stat.MyBattleAbility.CombatAI.SetActiveAI(true, stat.MyBattleAbility.AttackBehaviour);
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
