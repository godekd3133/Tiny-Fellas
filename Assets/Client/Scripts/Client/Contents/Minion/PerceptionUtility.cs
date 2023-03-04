using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amazon.GameLift.Model;
using UnityEngine;

public class PerceptionUtility : MonoBehaviour
{
    public static float perceptionRange = 10f;

    public const int RECOGNIZED_MINION_LIST_DEFAULT_CAPACITY = 100;

    public static List<Minion> GetPerceptedMinionList(Minion myMinion, ulong clientID)
    {
        var gameSession = GameSessionInstance.Instance;
        var playerDataMap = gameSession.PlayerDataByClientID;
        var recognizedMinionInstancesList = new List<Minion>(RECOGNIZED_MINION_LIST_DEFAULT_CAPACITY);

        foreach (var pair in playerDataMap)
        {
            if (pair.Key != clientID)
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

    public static List<Minion> GetPerceptedMinionListLocalClient(Minion myMinion)
    {
        //var gameSession = GameSessionInstance.Instance;
        //var playerDataMap = gameSession.PlayerDataByPlayerID;
        // var recognizedMinionInstancesList = new List<Minion>(RECOGNIZED_MINION_LIST_DEFAULT_CAPACITY);
        //
        // foreach (var each in SessionManager.instance.troopAdmins)
        // {
        //     if (each.minions.Any((Minion minion) => minion == myMinion) == false)
        //     {
        //         var minionInstanceList = each.minions;
        //         for (int i = 0; i < minionInstanceList.Count; i++)
        //         {
        //             var targetMinion = minionInstanceList[i];
        //             if ((targetMinion.transform.position - myMinion.transform.position).magnitude <= perceptionRange)
        //             {
        //                 recognizedMinionInstancesList.AddRange(minionInstanceList);
        //                 break;
        //             }
        //         }
        //     }
        // }
        // recognizedMinionInstancesList.Sort((Minion prev, Minion next) => { return (prev.transform.position - myMinion.transform.position).magnitude.CompareTo((next.transform.position - myMinion.transform.position).magnitude); });
        // return recognizedMinionInstancesList;

        return null;
    }
}
