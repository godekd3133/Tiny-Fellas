using UnityEngine;

public abstract class AttackBase : EnvLayer
{
    public abstract void Attack(UnitBase sender, UnitBase target, int damage, string dialog);
}