using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MinionStat", menuName = "ScriptableObjects/MinionStat")]
public class MinionStat : StatContainer
{
    [SerializeField] private BattleAbility battleAbility;
    private int? indexInDB;

    public int? IndexInDB
    {
        get => indexInDB;
        set
        {
            if (indexInDB == null) indexInDB = value;
        } 

    }
    public BattleAbility MyBattleAbility => battleAbility;

    private MinionStat()
    {
    }

    public MinionStat(MinionStat origin) : base(origin)
    {
        this.indexInDB = origin.IndexInDB;
        battleAbility = new(origin.battleAbility);
    }

}
