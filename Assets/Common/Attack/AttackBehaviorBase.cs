using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazonGameLift;

public class AtackBehaviourBase : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Minion owner;

    private MinionStat targetStat;
    private BattleAbility battleAbility;
    
    private bool isOnAttacking;
    public bool IsOnAttacking
    {
        get => isOnAttacking;
    }

    public bool AttackStart(Minion target, BattleAbility battleAbility)
    {
        this.battleAbility = battleAbility;
        if (IsInAttackRagne(target.transform) == false)
            return false;
        
        var targetStat = target.GetComponent<MinionStat>();
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

        isOnAttacking = true;
        animator.SetTrigger(battleAbility.AttackAnimationParameter);
        return true;
    }

    public bool IsInAttackRagne(Transform target)
    {
        return (gameObject.transform.position - target.position).magnitude <= battleAbility[EStatName.ATTACK_RAGNE].CurrentValue;
    }

    public virtual void ImpactDamage()
    {
        targetStat.TakeDamage(owner,battleAbility[EStatName.DAMAGE].CurrentValue);
    }
    
    // called as animation event
    public void AttackDone()
    {
        isOnAttacking = false;
    }
}
