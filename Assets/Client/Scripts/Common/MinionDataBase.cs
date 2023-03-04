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
   }  private void OnEnable()
   {
      hadInitiliazed = false;
   }

   private void Initialize()
   {
      dataByIndex = new Dictionary<int, MinionData>();
      for (int i = 0; i < minionDatas.Count; i++)
      {
         dataByIndex.Add(i,minionDatas[i]);
         minionDatas[i].IndexInContainer = i;
      }

      hadInitiliazed = true;
   }
}

[Serializable]
public class MinionData: IIndexContainable
{
   [SerializeField] private GameObject prefab;
   [SerializeField] private Sprite thumbnail;
   [SerializeField] private MinionStat stat;
   
   #if UNITY_EDITOR
   [SerializeField] private int indexInContainerNullCheckVariable = -1;
   #endif
   private int? indexInContainer;

   public GameObject Prefab => prefab;

   public Sprite Thumbnail => thumbnail;
   public MinionStat Stat => stat;

   public int? IndexInContainer
   {
      get => indexInContainer;
      set
      {
         if (indexInContainer == null)
         {
            indexInContainer = value;
#if UNITY_EDITOR
            indexInContainerNullCheckVariable = value.Value;
#endif
         }
      }
   }
   

   public MinionData()
   {
   }

   public MinionData(GameObject prefab,Sprite thumbnail , MinionStat stat, int? indexInContainer )
   {
      this.prefab = prefab;
      this.thumbnail = thumbnail;
      this.stat =  stat;
      if(indexInContainer == null) Assert.IsTrue(indexInContainer == null);    
      this.indexInContainer = indexInContainer;
      #if UNITY_EDITOR
      this.indexInContainerNullCheckVariable = indexInContainer.Value;
#endif
   }
}
