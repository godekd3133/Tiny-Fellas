using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleAbilityDatasBase", menuName = "ScriptableObjects/BattleAbilityDatasBase")]
public class BattleAbilityDatasBase : ScriptableObject
{
   [SerializeField] private List<BattleAbility> battleAbilities;

   private Dictionary<int, BattleAbility> dataByIndex;

   public IReadOnlyDictionary<int, BattleAbility> DataByInex
   {
      get
      {
         if (dataByIndex == null)
         {
            dataByIndex = new Dictionary<int, BattleAbility>();
            for(int i=0;i<battleAbilities.Count;i++) dataByIndex.Add(i,battleAbilities[i]);
         }

         return dataByIndex;
      }
   }
}
