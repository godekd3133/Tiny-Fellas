using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStat : MonoBehaviour
{
    private Dictionary<EStatName, Stat> statMap = new Dictionary<EStatName, Stat>();

    public bool IsDead
    {
        get => statMap[EStatName.HEALTH].CurrentValue <= statMap[EStatName.HEALTH].MinValue;
    }

    // TODO: apply buff system
    public virtual void TakeDamage(Minion attcker, float damage)
    {
        statMap[EStatName.HEALTH].CurrentValue -= damage;
    }
}
