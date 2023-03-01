using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class MinionInstanceStat : NetworkBehaviour
{
    private MinionStat originStat;
    private MinionStat currentStat;
    private BattleAbility originBattleAbility;
    private BattleAbility battleAbility;
    
    public BattleAbility MyBattleAbility => battleAbility;
    

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
        originBattleAbility = currentStat.MyBattleAbility;
        battleAbility = new(battleAbility);
        
        return true;
    }

    [ClientRpc]
    public void SetHealth_ClientRPC(float value)
    {
        
    }
    
#if UNITY_SERVER || UNITY_EDITOR
    public bool TakeDamage(Minion attacker,BattleAbility attackerBattleAbility)
    {
        if (IsDead) return false;
        
        currentStat[EStatName.HEALTH].CurrentValue -= attackerBattleAbility[EStatName.DAMAGE].CurrentValue;
        return true;
    }
    #endif  
}
