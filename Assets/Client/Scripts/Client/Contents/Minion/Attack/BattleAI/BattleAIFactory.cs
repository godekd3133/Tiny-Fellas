
using UnityEngine.Assertions;

public class BattleAIFactory : Singleton<BattleAIFactory>
{
    public BattleAI GetCombatAI(EComatAIName combatAIName)
    {
        switch (combatAIName)
        {
            case EComatAIName.MELEE: return new MeleeBattleAI();
            case EComatAIName.RAGNE: return new RangeBattleAI();
            default:
                Assert.IsTrue(true,
                    string.Format(
                        "Get CombatAI instace of {0}'s case is undefined check CombatAIFactory class's function GetCombatAI",
                        combatAIName.ToString()));
                return null;
        }
    }

}
