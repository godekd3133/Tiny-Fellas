using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AttackBehaviourProjectile : AttackBehaviourBase
{
    protected override void Attack(Minion target)
    {
        
        var instantiatedProjectile = Instantiate(MyBattleAbility.ProjectilePrefab);
        instantiatedProjectile.transform.position = transform.position;
        
    }
}
