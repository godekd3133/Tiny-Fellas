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


    [HideInInspector] public UnityEvent onStatChanged;
    public MinionInstanceStat stat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stat.MyBattleAbility.AttackBehaviour.SetOwner(this, animator);
    }

    public void Attack()
    {
        stat.MyBattleAbility.CombatAI.SetActiveAI(true, stat.MyBattleAbility.AttackBehaviour);
    }
}
