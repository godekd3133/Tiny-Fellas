using System;
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

[CreateAssetMenu(fileName = "BattleAbility", menuName = "ScriptableObjects/BattleAbility"),Serializable]
public class BattleAbility : StatContainer
{
    [SerializeField] private EMinionAnimationParameter attackAnimationParameter;
    [SerializeField] private EComatAIName combatAIName;
    [SerializeField] private ESkillPassiveName passiveSkillName;
    [SerializeField] private AttackBehaviourBase attackBehaviour;
    [SerializeField] private ProjectileBase projectilePrefab;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject inGameIcon;
    [SerializeField] private Sprite thumbnail;

    private BattleAI combatAI;
    private ASkillPassiveBase passiveSkill;
    private string attackAnimationParameterAsString;


    public Sprite Thumbnail => thumbnail;
    public GameObject InGameIcon => inGameIcon;
    public GameObject Attackeffect => attackEffect;
    public EMinionAnimationParameter AttackAnimationparameterEnum => attackAnimationParameter;
    public EComatAIName CombatAIName => combatAIName;
    public string AttackAnimationParameter
    {
        get
        {
            if (attackAnimationParameterAsString == null) attackAnimationParameterAsString = attackAnimationParameter.ToString();
            return new(attackAnimationParameterAsString);
        }
    }

    public BattleAI CombatAI
    {
        get
        {
            if (combatAI == null) combatAI = BattleAIFactory.Instace.GetCombatAI(combatAIName);
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
}

[Serializable]
public class BattleAbilityInstance : StatContainerInstance
{
    [SerializeField] private EMinionAnimationParameter attackAnimationParameter;
    [SerializeField] private EComatAIName combatAIName;
    [SerializeField] private ESkillPassiveName passiveSkillName;
    [SerializeField] private AttackBehaviourBase attackBehaviour;
    [SerializeField] private ProjectileBase projectilePrefab;
    [SerializeField] private GameObject attackEffect;

    private BattleAI combatAI;
    private ASkillPassiveBase passiveSkill;
    private string attackAnimationParameterAsString;


    public string AttackAnimationParameter
    {
        get
        {
            if (attackAnimationParameterAsString == null) attackAnimationParameterAsString = attackAnimationParameter.ToString();
            return new(attackAnimationParameterAsString);
        }
    }

    public BattleAI CombatAI
    {
        get
        {
            if (combatAI == null) combatAI = BattleAIFactory.Instace.GetCombatAI(combatAIName);
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


    private BattleAbilityInstance()
    {
    }

    public BattleAbilityInstance(BattleAbility origin) : base(origin)
    {
        attackAnimationParameter = origin.AttackAnimationparameterEnum;
        combatAIName = origin.CombatAIName;
        attackBehaviour = origin.AttackBehaviour;
    }
}
