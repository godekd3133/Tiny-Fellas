using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "MinionDataBase", menuName = "ScriptableObjects/MinionDataBase")]
public class MinionDataBase : ScriptableObject
{
   [SerializeField] private List<MinionData> minionDatas;
   private bool hadInitiliazed;

   private Dictionary<int, MinionData> dataByIndex;

   public IReadOnlyList<MinionData> MinionDatas
   {
      get
      {
         if (!hadInitiliazed)   Initialize();
         return minionDatas;
      }
   }

   public IReadOnlyDictionary<int, MinionData> DataByInex
   {
      get
      {
         if (!hadInitiliazed)   Initialize();
         return dataByIndex;
      }
   }

   private void Initialize()
   {
      dataByIndex = new Dictionary<int, MinionData>();
      for (int i = 0; i < minionDatas.Count; i++)
      {
         dataByIndex.Add(i,minionDatas[i]);
         minionDatas[i].IndexInDB = i;
      }

      hadInitiliazed = true;
   }
}

[Serializable]
public class MinionData
{
   [SerializeField] private GameObject prefab;
   [SerializeField] private Sprite thumbnail;
   [SerializeField] private MinionStat stat;
   private int? indexInDB;

   public GameObject Prefab => prefab;

   public Sprite Thumbnail => thumbnail;
   public MinionStat Stat => stat;

   public int? IndexInDB
   {
      get => indexInDB;
      set
      {
         if (indexInDB == null) indexInDB = value;
      }
   }
   

   public MinionData()
   {
   }

   public MinionData(GameObject prefab,Sprite thumbnail , MinionStat stat, int? indexInDB )
   {
      this.prefab = prefab;
      this.thumbnail = thumbnail;
      this.stat =  new(stat);
      Assert.IsTrue(indexInDB == null);      
      this.indexInDB = indexInDB;
   }
}
