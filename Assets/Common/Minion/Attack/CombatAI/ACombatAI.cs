using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ACombatAI : MonoBehaviour
{
    public Minion owner;
    public AttackBehaviourBase attackBehaviour;
    public float AIUpdateInterval = 0.5f;
    public List<Minion> recognizedMinionList;
    
    private Coroutine AICoroutine;

    public bool isOn
    {
        private set;
        get;
    }
    
    public void SetActiveAI(bool flag)
    {
        if (flag && !isOn)
        {
            AICoroutine = StartCoroutine(StartCombatAI());
            isOn = true;
        }
        else
        {
            StopCoroutine(AICoroutine);
            isOn = false;
        }
    }

    protected abstract IEnumerator StartCombatAI();

    public bool HasRecognizedEnemy()
    {
        if (recognizedMinionList == null) return false;
        
        return recognizedMinionList.Count > 0;
    }
}
