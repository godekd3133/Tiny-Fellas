using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionDataBase", menuName = "ScriptableObjects/MinionDataBase")]
public class MinionDataBase : ScriptableObject
{
   [SerializeField] private List<MinionData> minionDatas;

   private Dictionary<int, MinionData> dataByIndex;

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

   public GameObject Prefab => prefab;

   public Sprite Thumbnail => thumbnail;
}
