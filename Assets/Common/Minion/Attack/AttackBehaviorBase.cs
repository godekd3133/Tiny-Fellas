using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazonGameLift;

public class AttackBehaviourBase : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Minion owner;

    private MinionInstanceStat targetStat;
    private BattleAbility battleAbility;
    
    private bool isOnAttacking;
    public bool IsOnAttacking
    {
        get => isOnAttacking;
    }

    public virtual bool IsAttackable
    { 
        get => !IsOnAttacking;
    }

    public bool AttackStart(Minion target, BattleAbility battleAbility)
    {
        this.battleAbility = battleAbility;
        if (IsInAttackRagne(target.transform) == false)
            return false;
        
        var targetStat = target.GetComponent<MinionInstanceStat>();
        if (target.GetComponent<MinionStat>() == null)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but stat component is null",target.name));
            return false;
        }
        this.targetStat = targetStat;

        if (isOnAttacking)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but was already on attacking",target.name));
            return false;
        }

        Attack(target);
        return true;
    }

    private IEnumerator StartAttackDelay()
    {
        isOnAttacking = true;
        yield return new WaitForSeconds(battleAbility[EStatName.ATTACK_AFTER_DELAY].CurrentValue);
        isOnAttacking = false;
    }
    
    protected virtual void Attack(Minion target)
    {
        animator.SetTrigger(battleAbility.AttackAnimationParameter);
    }

    public bool IsInAttackRagne(Transform target)
    {
        return (gameObject.transform.position - target.position).magnitude <= battleAbility[EStatName.ATTACK_RAGNE].CurrentValue;
    }

    // called as animation event
    public virtual void ImpactDamage()
    {
        targetStat.TakeDamage(owner,battleAbility);
    }
    
    // called as animation event
    public void AttackDone()
    {
        isOnAttacking = false;
    }
}
