using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDeckDeserializeHelper : MonoWeakSingleton<MinionDeckDeserializeHelper>
{
    [SerializeField] private MinionDataBase minionDataBase;
    [SerializeField] private MinionStatDataBase minionStatDataBase;

    public MinionData GetMinionDeck(List<int> minionIndexList, List<int> statIndexList)
    {
        var deck = new List<MinionData>();
        for (int i = 0; i < minionIndexList.Count; i++)
            deck.Add(GetMinionDataInstance(minionIndexList[i], statIndexList[i]));

        return deck;
    }

    public MinionData GetMinionDataInstance(int minionIndex, int statIndex)
    {
        return new MinionData(minionDataBase.DataByInex[minionIndex], minionStatDataBase.DataByInex[statIndex]);
    }
}

