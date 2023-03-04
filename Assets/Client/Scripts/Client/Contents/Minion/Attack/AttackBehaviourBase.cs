using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Netcode;

public class AttackBehaviourBase : MonoBehaviour
{
    private Animator animator;
    private Minion owner;

    private MinionInstance targetStat;
    private BattleAbilityInstance battleAbility;
    
    private bool isOnAttacking;
    public bool IsOnAttacking
    {
        get => isOnAttacking;
    }

    public virtual bool IsAttackable
    { 
        get => !IsOnAttacking;
    }

    protected MinionInstance TargetStat => targetStat;
    protected BattleAbilityInstance MyBattleAbility => battleAbility;

    public void SetOwner(Minion owner, Animator animator)
    {
        this.owner = owner;
        this.animator = animator;
    }

    public void SetOwner(Minion owner)
    {
        this.owner = owner;
        animator = owner.GetComponent<Animator>();
    }
    
    
    public bool AttackStart(Minion target, BattleAbilityInstance battleAbility)
    {
        this.battleAbility = battleAbility;
        if (IsInAttackRagne(target.transform) == false)
            return false;
        
        var targetStat = target.GetComponent<MinionInstance>();
        if (target.GetComponent<MinionStat>() == null)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but stat component is null",target.name));
            return false;
        }
        this.targetStat = targetStat;

        if (isOnAttacking)
        {
            Logger.SharedInstance.Write(string.Format("{0} tried to damage {1}, but was already on attacking",target.name));
            return false;
        }

        Attack(target);
        StartAttackDelay();
        return true;
    }

    private async UniTask StartAttackDelay()
    {
        isOnAttacking = true;
        await UniTask.Delay(Convert.ToInt32(battleAbility[EStatName.ATTACK_AFTER_DELAY].CurrentValue * 1000));
        isOnAttacking = false;
    }

    [ClientRpc]
    protected void Attack_ClientRPC()
    {
        animator.SetTrigger(battleAbility.AttackAnimationParameter);
    }
    
    protected virtual void Attack(Minion target)
    {
        animator.SetTrigger(battleAbility.AttackAnimationParameter);
        if (NetworkManagerInstance.Instance.IsServer) Attack_ClientRPC();
        // NetworkMinionAnimationAdmin.Instance.PlayAnimation_ClientRPC(owner.OwnerClientId, owner.IndexInContainer.Value, battleAbility.AttackAnimationParameter);
    }

    public bool IsInAttackRagne(Transform target)
    {
        return (owner.transform.position - target.position).magnitude <= battleAbility[EStatName.ATTACK_RAGNE].CurrentValue;
    }

    // called as animation event
    public virtual void ImpactDamage()
    {
        #if UNITY_SERVER
        targetStat.TakeDamage(owner,battleAbility);
        #endif
    }
}
