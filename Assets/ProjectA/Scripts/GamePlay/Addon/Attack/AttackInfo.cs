using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class AttackInfo : ScriptableObject
{
    [SerializeField, Required] private AttackBase _attackPrefab;

    [SerializeField, Title("공격 범위")] private float _attackRange;

    [SerializeField, Title("쿨타임")] private float _reloadTime;

    [SerializeField, Title("공격 대기시간")] private float _preDelayTime;

    public AttackBase AttackPrefab => _attackPrefab;

    public float AttackRange => _attackRange;

    public float ReloadTime => _reloadTime;

    public float PreDelayTime => _preDelayTime;
}