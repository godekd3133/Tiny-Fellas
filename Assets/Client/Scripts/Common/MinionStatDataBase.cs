using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionStatDataBase", menuName = "ScriptableObjects/MinionStatDataBase")]
public class MinionStatDataBase : ScriptableObject
{
   [SerializeField] private List<MinionStat> minionStats;

   private Dictionary<int, MinionStat> dataByIndex;
   
   
   private bool hasInitialized;

   private void OnEnable()
   {
      hasInitialized = false;
   }

   public IReadOnlyList<MinionStat> MinionStats
   {
      get
      {
         if(!hasInitialized) Initilaize();
         return minionStats;
      }
   }

   public IReadOnlyDictionary<int, MinionStat> DataByInex
   {
      get
      {
         if(!hasInitialized) Initilaize();
         return dataByIndex;
      }
   }

   private void Initilaize()
   {
      dataByIndex = new Dictionary<int, MinionStat>();
      for (int i = 0; i < minionStats.Count; i++)
      {
         dataByIndex.Add(i,minionStats[i]);
         minionStats[i].IndexInContainer = i;
      }
   }
}
