using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatName
{
    NONE,
    HEALTH,
    MOVEMNT_CONTRIBUTION,
    DAMAGE,
    ATTACK_RAGNE,
    
}

[Serializable]
public class Stat
{
    private EStatName statName;
    private T maxValue;
    private T minValue;
    private T currentValue;

    public EStatName StatName => statName;
    public virtual T MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }

    public virtual T MinValue
    {
        get => minValue;
        set => minValue = value;
    }

    public virtual T CurrentValue
    {
        get => currentValue;
        set => currentValue = value;
    }

    private Stat() { }

    public Stat(EStatName statName, T maxValue, T minValue)
    {
        this.statName = statName;
        this.maxValue = maxValue;
        this.minValue = minValue;
        currentValue = maxValue;
    }
    
    public Stat(EStatName statName, T maxValue, T minValue, T currentValue)
    {
        this.statName = statName;
        this.maxValue = maxValue;
        this.minValue = minValue;
        this.currentValue = currentValue;
    }
}
