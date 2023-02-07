using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinionStat : MonoBehaviour
{
    private Dictionary<EStatName, Stat> statMap = new Dictionary<EStatName, Stat>();
    private UnityEvent onDead = new UnityEvent();

    public bool IsDead
    {
        get => statMap[EStatName.HEALTH].CurrentValue <= statMap[EStatName.HEALTH].MinValue;
    }

    public virtual void TakeDamage(Minion attcker, float damage)
    {
        statMap[EStatName.HEALTH].CurrentValue -= damage;
    }
}
