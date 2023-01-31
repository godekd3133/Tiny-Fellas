using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MoveBase
{
    public override void Move(UnitBase sender)
    {
        sender.transform.position += 0.5f * Vector3.left * Time.deltaTime;
    }

    public override void Stop(UnitBase sender)
    {
    }
}

