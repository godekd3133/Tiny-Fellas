using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBattleAI : BattleAI
{
    protected override async UniTask StartCombatAI(AttackBehaviourBase attackBehaviour, CancellationToken token)
    {
        var priorityQueue = new DistancePriorityQueue<Minion>(gameObject.transform.position);
        var waitUntill = new WaitUntil(HasRecognizedEnemy);
        while (true)
        {
            if (token.IsCancellationRequested == true)
                break;

            priorityQueue.Clear();
            priorityQueue.ChangeStandardPos(gameObject.transform.position);
            for (int i = 0; i < recognizedMinionList.Count; i++)
                priorityQueue.Enqueue(recognizedMinionList[i], recognizedMinionList[i].transform);

            var nearestMinion = priorityQueue.Peek;

            bool isInAttackRange = attackBehaviour.IsInAttackRagne(nearestMinion.transform);

            if (isInAttackRange)
            {
                Debug.Log("어택!");
                attackBehaviour.AttackStart(nearestMinion, owner.Stat.MyBattleAbility);
            }
            // else
            // {
            //     var enemyToMeDirection = (owner.transform.position - nearestMinion.transform.position).normalized;
            //     var dest = nearestMinion.transform.position +
            //                enemyToMeDirection * owner.stat.MyBattleAbility[EStatName.ATTACK_RAGNE].CurrentValue;
            //
            //     owner.agent.SetDestination(dest);
            // }
            await UniTask.Delay(TimeSpan.FromSeconds(AIUpdateInterval));
        }
    }
}
