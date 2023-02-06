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
    private Stat damageStat;
    
    private bool isOnAttacking;
    public bool IsOnAttacking
    {
        get => isOnAttacking;
    }

    public bool AttackStart(Minion target, Stat damageStat)
    {
        var targetStat = target.GetComponent<MinionStat>();
        if (target.GetComponent<MinionStat>() == null)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but stat component is null",target.name));
            return false;
        }

        if (isOnAttacking)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but was already on attacking",target.name));
            return false;
        }

        isOnAttacking = true;
        this.targetStat = targetStat;
        this.damageStat = damageStat;
        return true;
    }

    // TODO : apply buff system 
    // called as animation event
    public void ImpactDamage()
    {
        targetStat.TakeDamage(owner,damageStat.CurrentValue);
    }
    
    // called as animation event
    public void AttackDone()
    {
        isOnAttacking = false;
    }
}
