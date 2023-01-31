using UnityEngine;

public abstract class MoveBase : EnvLayer
{
    public abstract void Move(UnitBase sender);

    public abstract void Stop(UnitBase sender);
}