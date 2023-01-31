using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AddonAttackable : EnvLayer, ISetup<AttackInfo>
{
    public AttackInfo _attackInfo;

    public bool IsAttack;

    [SerializeField] protected UnitBase unit;

    public virtual void Init(AttackInfo table, UnitBase my)
    {
        Init(eLAYER_TYPE.Object);

        _attackInfo = table;
        unit = my;
    }

    public sealed override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (IsAttack)
            return;


        var near = unit.My.Stat.Type.GetFindNearTarget(gameObject);
        Debug.Log($"{gameObject.name} : 탐색중 ... : {unit.My.Stat.Type} : {near}");
        if (near)
        {

            if (IsInAttackRange(near))
            {
                Debug.Log($"{gameObject.name} : 가까운 적 발견 ...");

                IsAttack = true;
                StartCoroutine(AttackRoutineInternal(near, (int)unit.DAMAGE));
            }
        }
    }

    public bool IsInAttackRange(UnitBase other)
    {
        if (!other)
            return false;
        return (transform.position - other.transform.position).sqrMagnitude
            < _attackInfo.AttackRange * _attackInfo.AttackRange;
    }

    private IEnumerator AttackRoutineInternal(UnitBase target, int damage)
    {
        // 대기...
        if (_attackInfo.PreDelayTime > float.Epsilon)
            yield return new WaitForSeconds(_attackInfo.PreDelayTime);

        if (target)
        {
            IsAttack = true;

            var aI = Instantiate(_attackInfo.AttackPrefab);
            aI.transform.position = transform.position;
            aI.Attack(unit, target, damage, "이얏");

            yield return ReloadWaitRoutine();
        }
    }

    private IEnumerator ReloadWaitRoutine()
    {
        yield return new WaitForSeconds(_attackInfo.ReloadTime);
        IsAttack = false;
    }

    public void Setup(AttackInfo table)
    {
        _attackInfo = table;
        IsAttack = false;
    }
}