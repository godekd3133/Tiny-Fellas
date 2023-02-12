using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(MinionInstanceStat))]
public class Minion : MonoBehaviour
{
    public PlayerData ownerPlayer;
    public float moveSpeed;
    public NavMeshAgent agent;
    public Animator animator;

    private Action<Minion> beforeAttack;
    private Action<Minion> afterAttack;
    private Action<Minion> befroeDamaged;
    private Action<Minion> afterDamaged;


    [HideInInspector] public UnityEvent onStatChanged;
    private MinionInstanceStat stat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stat.MyBattleAbility.AttackBehaviour.SetOwner(this, animator);
    }

    public void Attack()
    {
        beforeAttack(this);
        stat.MyBattleAbility.CombatAI.SetActiveAI(true, stat.MyBattleAbility.AttackBehaviour);
        afterAttack(this);
    }

    public bool TakdeDamage()
    {
        befroeDamaged(this);
        var flag = stat.TakeDamage(this, stat.MyBattleAbility);
        afterDamaged(this);

        return true;
    }

    public void SubscribeBeforeAttack(Action<Minion> action)
    {
        beforeAttack += action;
    }
    
    public void SubscribeAfterAttack(Action<Minion> action)
    {
        afterAttack += action;
    }
    
    public void SubscribeBeforeDamaged(Action<Minion> action)
    {
        befroeDamaged += action;
    }
    
    public void SubscribeAfterDamaged(Action<Minion> action)
    {
        afterDamaged += action;
    }
    
    public void UnSubscribeBeforeAttack(Action<Minion> action)
    {
        beforeAttack -= action;
    }
    
    public void UnSubscribeAfterAttack(Action<Minion> action)
    {
        afterAttack -= action;
    }
    
    
    public void UnSubscribeBeforeDamaged(Action<Minion> action)
    {
        befroeDamaged -= action;
    }
    
    public void UnSubscribeAfterDamaged(Action<Minion> action)
    {
        afterDamaged -= action;
    }
    
}
