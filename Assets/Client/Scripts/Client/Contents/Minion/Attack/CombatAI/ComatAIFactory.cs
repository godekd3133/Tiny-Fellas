
using UnityEngine.Assertions;

public class CombatAIFactory : Singleton<CombatAIFactory>
{
    public ACombatAI GetCombatAI(EComatAIName combatAIName)
    {
        switch (combatAIName)
        {
          case EComatAIName.MELEE: return new CombatAIMelee(); 
          case EComatAIName.RAGNE: return new CombatAIRange(); 
          default:
              Assert.IsTrue(true,
                  string.Format(
                      "Get CombatAI instace of {0}'s case is undefined check CombatAIFactory class's function GetCombatAI",
                      combatAIName.ToString()));
              return null;
        }
    }
    
}
