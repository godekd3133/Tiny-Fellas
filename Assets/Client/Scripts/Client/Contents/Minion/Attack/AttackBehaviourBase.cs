using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class AttackBehaviourBase : NetworkBehaviour
{
    private Animator animator;
    private Minion owner;

    [SerializeField] private Minion target;
    [SerializeField] private BattleAbilityInstance battleAbility;

    [SerializeField] private bool isOnAttacking;
    public bool IsOnAttacking
    {
        get => isOnAttacking;
    }

    public virtual bool IsAttackable
    {
        get => !IsOnAttacking;
    }

    protected Minion Target => target;
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
        Debug.Log("attack start in server");
        this.battleAbility = battleAbility;
        if ((target.transform.position - gameObject.transform.position).magnitude <= battleAbility[EStatName.ATTACK_RAGNE].CurrentValue)
            return false;

        var targetStat = target.GetComponent<MinionInstance>();
        if (targetStat == null)
        {
            Logger.SharedInstance.Write(string.Format("{0} tride to damage {1}, but stat component is null", target.name));
            return false;
        }
        this.target = target;

        if (isOnAttacking)
        {
            Logger.SharedInstance.Write(string.Format("{0} tried to damage {1}, but was already on attacking", target.name));
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

    public void AttackAnimation()
    {
        Debug.Log("start attack animation");
        animator.SetTrigger(battleAbility.AttackAnimationParameter);
    }

    protected virtual void Attack(Minion target)
    {
        Debug.Log("attack order in server");
        AttackAnimation();
    }

    public virtual void ImpactDamage()
    {
        target.TakeDamage(battleAbility);
    }
}
