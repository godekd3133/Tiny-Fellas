using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilBase : MonoBehaviour
{
    [SerializeField] protected AbilInfo info;

    public virtual void Skill(AbilInfo info)
    {
        this.info = info;
    }
}
