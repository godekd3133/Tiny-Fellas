using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public abstract class BattleAI : MonoBehaviour
{
    public Minion owner;
    public float AIUpdateInterval = 0.5f;
    public List<Minion> recognizedMinionList;

    CancellationTokenSource tokenSource = new CancellationTokenSource();
    public bool isOn
    {
        private set;
        get;
    }

    public void SetActiveAI(bool flag, AttackBehaviourBase attackBehaviour)
    {
        if (flag && !isOn)
        {
            tokenSource = new CancellationTokenSource();

            StartCombatAI(attackBehaviour, tokenSource.Token);
            isOn = true;
        }
        else
        {
            tokenSource.Cancel();
            isOn = false;
        }
    }

    protected abstract UniTask StartCombatAI(AttackBehaviourBase attackBehaviour, CancellationToken token);
    public List<Minion> UpdateRecognizedEnemy()
    {
        recognizedMinionList = PerceptionUtility.GetPerceptedMinionListLocalClient(owner);
        return recognizedMinionList;
    }
    public bool HasRecognizedEnemy()
    {
        if (recognizedMinionList == null) return false;

        return recognizedMinionList.Count > 0;
    }
}
