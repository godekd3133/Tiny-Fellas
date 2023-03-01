using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MinionStat", menuName = "ScriptableObjects/MinionStat")]
public class MinionStat : StatContainer, IIndexContainable
{
    [SerializeField] private BattleAbility battleAbility;
    private int? indexInContainer;

    public int? IndexInContainer
    {
        get => indexInContainer;
        set
        {
            if (indexInContainer == null) indexInContainer = value;
        } 

    }
    public BattleAbility MyBattleAbility => battleAbility;

    private MinionStat()
    {
    }

    public MinionStat(MinionStat origin) : base(origin)
    {
        indexInContainer = origin.indexInContainer;
        battleAbility = new(origin.battleAbility);
    }

}
