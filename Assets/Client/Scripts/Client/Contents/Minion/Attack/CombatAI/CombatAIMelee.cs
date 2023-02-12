using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatAIMelee : ACombatAI
{
    protected override IEnumerator StartCombatAI(AttackBehaviourBase attackBehaviour)
    {
        var priorityQueue = new DistancePriorityQueue<Minion>(gameObject.transform.position);
        var waitUntill = new WaitUntil(HasRecognizedEnemy);
        while (true)
        {
            yield return waitUntill;
            priorityQueue.Clear();
            priorityQueue.ChangeStandardPos(gameObject.transform.position);
            for(int i=0;i<recognizedMinionList.Count;i++)
                priorityQueue.Enqueue(recognizedMinionList[i], recognizedMinionList[i].transform);

            var nearestMinion = priorityQueue.Peek;

            bool isInAttackRange = attackBehaviour.IsInAttackRagne(nearestMinion.transform);

            if (isInAttackRange)
            {
                attackBehaviour.AttackStart(nearestMinion, owner.stat.MyBattleAbility);
            }
            // else
            // {
            //     var enemyToMeDirection = (owner.transform.position - nearestMinion.transform.position).normalized;
            //     var dest = nearestMinion.transform.position +
            //                enemyToMeDirection * owner.stat.MyBattleAbility[EStatName.ATTACK_RAGNE].CurrentValue;
            //
            //     owner.agent.SetDestination(dest);
            // }
        }
        yield break;
    }
}
