using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using UnityEngine;

public class PerceptionUtility : MonoBehaviour
{
    public static float perceptionRange = 10f;

    public const int RECOGNIZED_MINION_LIST_DEFAULT_CAPACITY = 100;
    
    public static List<Minion> GetPerceptedMinionList(Minion myMinion, PlayerSession playerSession)
    {
        var gameSession = GameSessionInstance.Instance;
        var playerDataMap = gameSession.PlayerDataByPlayerID;
        var recognizedMinionInstancesList = new List<Minion>(RECOGNIZED_MINION_LIST_DEFAULT_CAPACITY);
        
        foreach (var pair in playerDataMap)
        {
            if (pair.Key != playerSession.PlayerId)
            {
                var minionInstanceList = pair.Value.MinionInstanceList;
                for (int i = 0; i < minionInstanceList.Count; i++)
                {
                    var targetMinion = minionInstanceList[i];
                    if ((targetMinion.transform.position - myMinion.transform.position).magnitude <= perceptionRange)
                    {
                        recognizedMinionInstancesList.AddRange(minionInstanceList);
                        break;
                    }
                }
            }
        }

        return recognizedMinionInstancesList;
    }
}
