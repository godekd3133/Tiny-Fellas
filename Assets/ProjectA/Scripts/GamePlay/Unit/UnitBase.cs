using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : EnvLayer, ISetup<UnitInfo>
{
    public static HashSet<UnitBase> Units = new HashSet<UnitBase>();

    public UnitInfo My;

    public bool IsDied 
    {
        get
        {
            if (_hp <= 0)
                return true;
            else
                return false;
        }
    }

    public float HP
    {
        get => _hp;
        set
        {
            if (_hp != value || _hp + value <= My.Stat.HP)
            {
                _hp = value;
            }
            if (value <= 0)
                OnRelease();
        }
    }

    public float DAMAGE
    {
        get => _damage;
        set
        {
            _damage = value;
        }
    }

    public float DEFENSE
    {
        get => _defense;
        set
        {
            _defense = value;
        }
    }

    public float SPEED
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
    
    private string _name;
    private float _hp;
    private float _damage;
    private float _defense;
    private float _speed;

    private void Start() => OnSpawn();

    //private void OnEnable() => OnSpawn();

    private void OnDestroy() => OnRelease();

    private void OnDisable() => OnRelease();

    public virtual void OnSpawn()
    {
        if (Units.Contains(this))
            return;

        Init(eLAYER_TYPE.Object);

        Units.Add(this);

        _hp      = My.Stat.HP;
        _damage  = My.Stat.Damage;
        _defense = My.Stat.Defense;
        _speed   = My.Stat.Speed;

        Debug.Log($"{gameObject.name} : Spawn");
    }

    public virtual void OnRelease()
    {
        if (!Units.Contains(this))
            return;

        //Units.Remove(this);
        ObjectPooler.ReturnToPool(gameObject);
        Debug.Log($"{gameObject.name} : Release");
    }

    public void Setup(UnitInfo table)
    {
        My = table;

        _name = My.Name;
        _hp = My.Stat.HP;
        _damage = My.Stat.Damage;
        _defense = My.Stat.Defense;
        _speed = My.Stat.Speed;

        switch(_name)
        {
            case "Enemy":
                //gameObject.AddComponent<EnemyDefaultMove>().Setup(new Vector2(-1,0), _speed);
                gameObject.AddComponent<SpriteRenderer>();
                break;
        }
        gameObject.GetComponentNoAlloc<SpriteRenderer>().sprite = table.Portrait;
        gameObject.GetComponentNoAlloc<SpriteRenderer>().color = table.Color;

        Debug.Log($"{gameObject.name} : Setup");
        OnSpawn();
    }
}
