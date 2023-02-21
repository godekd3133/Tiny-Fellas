using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinionDeployable 
{
    public IReadOnlyList<MinionData> MinionDeck
    {
        get;
    }

    public IReadOnlyList<Minion> MinionInstanceList
    {
        get;
    }
}
