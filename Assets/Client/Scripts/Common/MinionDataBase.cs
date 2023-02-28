using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionDataBase", menuName = "ScriptableObjects/MinionDataBase")]
public class MinionDataBase : ScriptableObject
{
   [SerializeField] private List<MinionData> minionDatas;

   private Dictionary<int, MinionData> dataByIndex;

   public IReadOnlyList<MinionData> MinionDatas => minionDatas;
   public IReadOnlyDictionary<int, MinionData> DataByInex
   {
      get
      {
         if (dataByIndex == null)
         {
            dataByIndex = new Dictionary<int, MinionData>();
            for(int i=0;i<minionDatas.Count;i++) dataByIndex.Add(i,minionDatas[i]);
         }

         return dataByIndex;
      }
   }
}

[Serializable]
public class MinionData
{
   [SerializeField] private GameObject prefab;
   [SerializeField] private Sprite thumbnail;
   [SerializeField] private MinionStat stat;

   public GameObject Prefab => prefab;

   public Sprite Thumbnail => thumbnail;
   public MinionStat Stat => stat;

   public MinionData()
   {
   }

   public MinionData(MinionData origin)
   {
      prefab = origin.Prefab;
      thumbnail = origin.Thumbnail;
      stat = origin.Stat;
   }
   
   public MinionData(GameObject prefab,MinionData origin  )
   {
      this.prefab = prefab;
      thumbnail = origin.Thumbnail;
   }
   
   public MinionData(GameObject prefab,MinionData origin , MinionStat stat )
   {
      this.prefab = prefab;
      thumbnail = origin.Thumbnail;
      this.stat = stat;
   }
}
