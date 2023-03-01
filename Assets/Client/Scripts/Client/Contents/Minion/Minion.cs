using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    public List<Minion> recognizedEnemies;
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


    [HideInInspector] public UnityEvent onStatChanged;
    private MinionInstanceStat stat;

    public MinionInstanceStat Stat => stat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //        stat.MyBattleAbility.AttackBehaviour.SetOwner(this, animator);
        //      stat.MyBattleAbility.PassiveSkill.ApplyEffect(this);
        recognizedEnemies = new List<Minion>();

    }

    private void Start()
    {
        DetectEnemyUpdate().Forget();
    }

    private async UniTask DetectEnemyUpdate()
    {
        const float updateInterval = 0.15f;
        while (true)
        {
            recognizedEnemies = PerceptionUtility.GetPerceptedMinionListLocalClient(this);
            await UniTask.Delay(TimeSpan.FromSeconds(updateInterval));
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
        stat.MyBattleAbility.CombatAI.SetActiveAI(true, stat.MyBattleAbility.OriginAttackBehaviour);
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
