using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class MinionInstance : NetworkBehaviour
{
    private MinionStatInstance originStat;
    private MinionStatInstance currentStat;
    private BattleAbilityInstance originBattleAbility;
    private BattleAbilityInstance battleAbility;
    private AttackBehaviourBase attackBehaviour;
    
    public BattleAbilityInstance MyBattleAbility => battleAbility;
    public AttackBehaviourBase MyAttackBehaviour => attackBehaviour;
    

    public bool IsDead => currentStat[EStatName.HEALTH].CurrentValue <= currentStat[EStatName.HEALTH].MinValue;
    private void OnEnable()
    {
        originStat = null;
    }
    
    public bool AssignOriginStat(MinionStat originStat, GameObject owner)
    {
        return AssignOriginStat(originStat, owner.GetComponent<Minion>());
    }

    public bool AssignOriginStat(MinionStat originStat, Minion owner)
    {
        this.originStat = new MinionStatInstance(originStat);
        currentStat  = new MinionStatInstance(originStat);
        originBattleAbility = this.originStat.MyBattleAbility;
        battleAbility = currentStat.MyBattleAbility;
        
        var preCreatedAttackbehaviour = gameObject.GetComponent<AttackBehaviourBase>();
        if(preCreatedAttackbehaviour != null) Destroy(preCreatedAttackbehaviour); 
        attackBehaviour = gameObject.AddComponent(battleAbility.AttackBehaviour.GetType()) as AttackBehaviourBase;
        
        attackBehaviour.SetOwner(owner);
        
        return true;
    }

    public void Attack(Minion target)
    {
        attackBehaviour.AttackStart(target, battleAbility);
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
