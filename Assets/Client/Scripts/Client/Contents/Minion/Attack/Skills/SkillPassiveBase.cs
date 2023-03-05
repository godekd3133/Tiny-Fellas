using System.Collections;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using UnityEngine;

public abstract class ASkillPassiveBase
{
    // subscribe this function to Minion's callback members
    public void ApplyEffect(Minion minion)
    {
        minion.OnPreDamaged.AddListener(BeforeDamagedEffect);
        minion.OnPreAttack.AddListener(BeforeAttackEffect);
        minion.OnPostDamaged.AddListener(AffterDamagedEffect);
        minion.OnPostAttack.AddListener(AfterAttackEffect);
    }

    public void DettachEffect(Minion minion)
    {
        minion.OnPreDamaged.RemoveListener(BeforeDamagedEffect);
        minion.OnPreAttack.RemoveListener(BeforeAttackEffect);
        minion.OnPostDamaged.RemoveListener(AffterDamagedEffect);
        minion.OnPostAttack.RemoveListener(AfterAttackEffect);
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
