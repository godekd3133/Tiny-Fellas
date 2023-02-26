using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionStatDataBase", menuName = "ScriptableObjects/MinionStatDataBase")]
public class MinionStatDataBase : ScriptableObject
{
   [SerializeField] private List<MinionStat> minionStats;

   private Dictionary<int, MinionStat> dataByIndex;

   public IReadOnlyDictionary<int, MinionStat> DataByInex
   {
      get
      {
         if (dataByIndex == null)
         {
            dataByIndex = new Dictionary<int, MinionStat>();
            for(int i=0;i<minionStats.Count;i++) dataByIndex.Add(i,minionStats[i]);
         }

         return dataByIndex;
      }
   }
}
