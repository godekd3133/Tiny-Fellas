using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MinionStat", menuName = "ScriptableObjects/MinionStat")]
public class MinionStat : StatContainer
{
    [SerializeField] private BattleAbility battleAbility;
    //TODO: ACombatAI member

    private MinionStat()
    {
    }

    public MinionStat(MinionStat origin) : base(origin)
    {
        battleAbility = new(origin.battleAbility);
    }

}
