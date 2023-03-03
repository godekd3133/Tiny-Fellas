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

public enum EComatAIName
{
    MELEE,
    RAGNE,
}

[CreateAssetMenu(fileName = "BattleAbility", menuName = "ScriptableObjects/BattleAbility")]
public class BattleAbility : StatContainer
{
    [SerializeField] private EMinionAnimationParameter attackAnimationParameter;
    [SerializeField] private EComatAIName combatAIName;
    [SerializeField] private ESkillPassiveName passiveSkillName;
    [SerializeField] private AttackBehaviourBase attackBehaviour;
    [SerializeField] private ProjectileBase projectilePrefab;

    private ACombatAI combatAI;
    private ASkillPassiveBase passiveSkill;
    private string attackAnimationParameterAsString;
    
    
    public string AttackAnimationParameter 
    {
        get
        {
           if(attackAnimationParameterAsString == null) attackAnimationParameterAsString = attackAnimationParameter.ToString();
               return new(attackAnimationParameterAsString);
        }
    }

    public ACombatAI CombatAI
    {
        get
        {
            if (combatAI == null) combatAI = CombatAIFactory.Instace.GetCombatAI(combatAIName);
            return combatAI;
        }
    }

    public ASkillPassiveBase PassiveSkill
    {
        get
        {
            if (passiveSkill == null) SkillPassiveFactory.Instace.GetSkill(passiveSkillName);
            return passiveSkill;
        }
    }
    
    public AttackBehaviourBase AttackBehaviour => attackBehaviour;
    public ProjectileBase ProjectilePrefab => projectilePrefab;
    

    private BattleAbility()
    {
    }

    public BattleAbility(BattleAbility origin) : base(origin)
    {
        attackAnimationParameter = origin.attackAnimationParameter;
        combatAIName = origin.combatAIName;

        var newAttackBehaviourPrefab = Instantiate(origin.AttackBehaviour);
        attackBehaviour = newAttackBehaviourPrefab;
    }
}
