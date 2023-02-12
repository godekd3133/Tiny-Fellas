using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CombatAIRange : ACombatAI
{
    protected override IEnumerator StartCombatAI(AttackBehaviourBase battleAbility)
    {
        yield break;
    }
}
