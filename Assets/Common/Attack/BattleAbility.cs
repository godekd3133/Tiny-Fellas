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

[CreateAssetMenu(fileName = "BattleAbility", menuName = "ScriptableObjects/BattleAbility")]
public class BattleAbility : StatContainer
{
    [SerializeField] private EMinionAnimationParameter attackAnimationParameter;

    private string attackAnimationParameterAsString;
    public string AttackAnimationParameter 
    {
        get
        {
           if(attackAnimationParameterAsString == null) attackAnimationParameterAsString = attackAnimationParameter.ToString();
               return new(attackAnimationParameterAsString);
        }
    }

    private BattleAbility()
    {
    }

    public BattleAbility(BattleAbility origin) : base(origin)
    {
        attackAnimationParameter = origin.attackAnimationParameter;
    }
}
