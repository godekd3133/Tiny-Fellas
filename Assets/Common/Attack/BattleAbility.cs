using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EMinionAnimationParameter
{
    JUMP,
    HI,
    ATTACK01,
    ATTACK02,
    PROJECTILE_ATACK,
    SPIN_ATTACK,
    CAST_SPELL,
    ROAR,
}
public class BattleAbility : SerializedScriptableObject
{
    [SerializeField] private EMinionAnimationParameter attackAnimationParameter;

    private string attackAnimationParameterAsString;
    public string AttackAnimationParameter => new (attackAnimationParameterAsString);

    private void Awake()
    {
        attackAnimationParameterAsString = attackAnimationParameter.ToString();
    }
}
