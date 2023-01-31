using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitStat
{
    public eUNIT_TYPE Type;

    public float HP;
    public float Damage;
    public float Defense;
    public float Speed;

    public UnitStat(float hp, float damage, float defense, float speed)
    {
        _ = hp;
        _ = damage;
        _ = defense;
        _ = speed;

        Debug.Log($"HP : {HP} / DF : {Defense} / Speed : {Speed}");
    }
}

[CreateAssetMenu]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private UnitStat _stat;

    [SerializeField] private Sprite _portrait;

    [SerializeField] private Color _color;

    public string Name => _name;

    public UnitStat Stat => _stat;

    public Sprite Portrait => _portrait;

    public Color Color => _color;
}
