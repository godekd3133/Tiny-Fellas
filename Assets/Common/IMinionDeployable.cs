using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinionDeployable 
{
    public IReadOnlyList<Minion> MinionDeck
    {
        get;
    }

    public IReadOnlyList<Minion> MinionInstanceList
    {
        get;
    }
}
