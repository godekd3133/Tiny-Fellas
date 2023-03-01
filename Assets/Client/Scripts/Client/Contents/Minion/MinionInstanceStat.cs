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
        this.originStat = originStat;
        currentStat = new(originStat);
        originBattleAbility = currentStat.MyBattleAbility;
        battleAbility = new(battleAbility);
        
        var preCreatedAttackbehaviour = gameObject.GetComponent<AttackBehaviourBase>();
        if(preCreatedAttackbehaviour != null) Destroy(preCreatedAttackbehaviour);
        gameObject.AddComponent(battleAbility.AttackBehaviour.GetType());
        
        return true;
    }

    [ClientRpc]
    public void SetHealth_ClientRPC(float value)
    {
        currentStat[EStatName.HEALTH].CurrentValue = value;
    }
    
    [ClientRpc]
    public void AssignOriginStat_ClientRPC(int statIndexInDB)
    {
        AssignOriginStat(MinionDataBaseIngame.Instance.MinionStatDataByIndex[statIndexInDB]);
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
