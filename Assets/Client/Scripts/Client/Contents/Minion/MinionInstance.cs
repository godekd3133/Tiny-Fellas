using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class MinionInstance : NetworkBehaviour
{
    private MinionStatInstance originStat;
    private MinionStatInstance currentStat;
    private BattleAbilityInstance originBattleAbility;
    private BattleAbilityInstance battleAbility;
    
    public BattleAbilityInstance MyBattleAbility => battleAbility;
    

    public bool IsDead => currentStat[EStatName.HEALTH].CurrentValue <= currentStat[EStatName.HEALTH].MinValue;
    private void OnEnable()
    {
        originStat = null;
    }

    public bool AssignOriginStat(MinionStat originStat)
    {
        this.originStat = new MinionStatInstance(originStat);
        currentStat  = new MinionStatInstance(originStat);
        originBattleAbility = this.originStat.MyBattleAbility;
        battleAbility = currentStat.MyBattleAbility;
        
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
    
#if UNITY_SERVER || UNITY_EDITOR
    public bool TakeDamage(Minion attacker,BattleAbilityInstance attackerBattleAbility)
    {
        if (IsDead) return false;
        
        currentStat[EStatName.HEALTH].CurrentValue -= attackerBattleAbility[EStatName.DAMAGE].CurrentValue;
        SetHealth_ClientRPC(currentStat[EStatName.HEALTH].CurrentValue);
        return true;
    }
    #endif  
}
