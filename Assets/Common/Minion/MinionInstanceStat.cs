using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MinionInstanceStat : MonoBehaviour
{
    private MinionStat originStat;
    private MinionStat currentStat;

    public bool IsDead => currentStat[EStatName.HEALTH].CurrentValue <= currentStat[EStatName.HEALTH].MinValue;
    private void OnEnable()
    {
        originStat = null;
    }

    public bool AssignOriginStat(MinionStat originStat)
    {
        if (this.originStat != null)
            return false;
        
        this.originStat = originStat;
        currentStat = new(originStat);
        return true;
    }

    public bool TakeDamage(Minion attacker,BattleAbility attackerBattleAbility)
    {
        if (IsDead) return false;
        
        currentStat[EStatName.HEALTH].CurrentValue -= attackerBattleAbility[EStatName.DAMAGE].CurrentValue;
        return true;
    }
}
