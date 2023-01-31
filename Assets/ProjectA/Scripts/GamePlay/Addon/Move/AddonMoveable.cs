using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddonMoveable : EnvLayer, ISetup<MoveInfo>
{
    public MoveInfo _moveInfo;

    public bool IsMoving
    {
        get => isMoving;
    }

    public float Speed
    {
        get => curSpeed;
        set => curSpeed = value;
    }

    public Transform Flip
    {
        get => flip;
        set
        {
            flip = value;
        }
    }

    [SerializeField] protected float _maxSpeed;

    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected Transform flip;

    [SerializeField] protected UnitBase unit;

    private float curSpeed;

    private bool isMoving;

    private void Start() => Init();

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        _moveInfo.MovePrefab.Move(unit);
    }

    public virtual void Init(MoveInfo table, UnitBase my)
    {
        Init(eLAYER_TYPE.Object);

        _moveInfo = table;
        unit = my;
    }

    public void FlipByDirection(Vector2 dir)
    {
        if (flip)
        {
            dir = EnvCamera.Current.GetRelativeInverse(dir);
            flip.localScale = new Vector3(Mathf.Sign(dir.x * -1f), 1, 1);
        }
    }

    public virtual void Stop() 
    {
        _moveInfo.MovePrefab.Stop(unit);
    }

    public void Setup(MoveInfo table)
    {
        _moveInfo = table;
        isMoving = false;
    }
}