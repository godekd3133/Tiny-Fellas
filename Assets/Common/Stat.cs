using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EStatName
{
    HEALTH,
    MOVEMENT_CONTRIBUTION,
    DAMAGE,
    ATTACK_RAGNE,
}

[Serializable]
[CreateAssetMenu(fileName = "Stat", menuName = "ScriptableObjects/Stat")]
public class Stat : ScriptableObject
{
    [SerializeField]
    private EStatName statName;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float minValue;
    [SerializeField]
    private float currentValue;

    public EStatName StatName => statName;
    public virtual float MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }

    public virtual float MinValue
    {
        get => minValue;
        set => minValue = value;
    }

    public virtual float CurrentValue
    {
        get => currentValue;
        set => currentValue = value;
    }

    public bool IsInRange(float value)
    {
        return value >= minValue && value < maxValue;
    }
}
