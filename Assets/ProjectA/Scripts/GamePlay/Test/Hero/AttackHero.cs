using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHero : AttackBase
{
    public override void Attack(UnitBase sender, UnitBase target, int damage, string dialog)
    {
        Debug.Log($"{sender.name} 가 {target.name} 에게 {damage} 데미지로 {dialog} 라고 말하면서 공격함 ");
        
        target.HP -= damage;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        transform.position += 1f * Vector3.left * Time.deltaTime;
    }
}
