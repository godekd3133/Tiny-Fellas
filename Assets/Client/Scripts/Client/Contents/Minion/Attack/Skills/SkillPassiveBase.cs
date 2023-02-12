using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASkillPassiveBase 
{
    // subscribe this function to Minion's callback members
    public void ApplyEffect(Minion minion)
    {
        minion.SubscribeBeforeDamaged(BeforeDamagedEffect);
        minion.SubscribeBeforeAttack(BeforeAttackEffect);
        minion.SubscribeAfterDamaged(AffterDamagedEffect);
        minion.SubscribeAfterAttack(AfterAttackEffect);
    }

    public void DettachEffect(Minion minion)
    {
        minion.UnSubscribeBeforeDamaged(BeforeDamagedEffect);
        minion.UnSubscribeBeforeAttack(BeforeAttackEffect);
        minion.UnSubscribeAfterDamaged(AffterDamagedEffect);
        minion.UnSubscribeAfterAttack(AfterAttackEffect);
    }

    protected virtual void BeforeAttackEffect(Minion minion)
    {
        
    }
    
    protected virtual void AfterAttackEffect(Minion minion)
    {
        
    }
    
    protected virtual void BeforeDamagedEffect(Minion minion)
    {
        
    }
    
    protected virtual void AffterDamagedEffect(Minion minion)
    {
        
    }
}
