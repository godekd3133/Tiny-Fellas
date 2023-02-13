using System.Collections;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using UnityEngine;

public abstract class ASkillPassiveBase
{
    // subscribe this function to Minion's callback members
    public void ApplyEffect(Minion minion)
    {
        minion.befroeDamaged.AddListener(BeforeDamagedEffect);
        minion.beforeAttack.AddListener(BeforeAttackEffect);
        minion.afterDamaged.AddListener(AffterDamagedEffect);
        minion.afterAttack.AddListener(AfterAttackEffect);
    }

    public void DettachEffect(Minion minion)
    {
        minion.befroeDamaged.RemoveListener(BeforeDamagedEffect);
        minion.beforeAttack.RemoveListener(BeforeAttackEffect);
        minion.afterDamaged.RemoveListener(AffterDamagedEffect);
        minion.afterAttack.RemoveListener(AfterAttackEffect);
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
