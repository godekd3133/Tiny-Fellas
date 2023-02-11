using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum TroopState
{
    IDLE = 0,
    MOVE = 1,
    CHASE = 2,
    BATTLE = 3
}
public class TroopAdmin : MonoBehaviour
{
    [ShowInInspector, ReadOnly] public List<Minion> minions { get; private set; }
    [ShowInInspector, ReadOnly] public Minion leaderMinion { get; private set; }
    [ShowInInspector, ReadOnly] public TroopState troopState { get; private set; }

    [HideInInspector] public UnityEvent<Minion> onPostMinionAdded;
    [HideInInspector] public UnityEvent<TroopState> onPostTroopUpdated;
    [HideInInspector] public UnityEvent<TroopState> onPostTroopStateChanged;
    void Awake()
    {
        troopState = TroopState.IDLE;
        minions = new List<Minion>();
        leaderMinion = null;
    }

    void Start()
    {
        //Test
        for (int i = 0; i < transform.childCount; i++)
        {
            minions.Add(transform.GetChild(i).GetComponent<Minion>());
        }
        leaderMinion = minions[0];
    }

    void Update()
    {
        if (troopState == TroopState.IDLE)
        {
            foreach (var each in minions)
            {
                if (each != leaderMinion && each.agent.destination != leaderMinion.transform.position)
                {
                    each.agent.stoppingDistance = 2.5f;
                    each.agent.SetDestination(leaderMinion.transform.position);
                }
            }
        }
        else if (troopState == TroopState.MOVE)
        {
            // 컨트롤중에는 유저의 컨트롤이 최우선이기에 자발적으로 특정한 행동을 하지 않음.
        }

        onPostTroopUpdated.Invoke(troopState);
    }

    /// <summary>
    /// 요청한 스테이트로 부터 가장 이상적인 형태로 바로 업데이트 해주는 함수
    /// </summary>
    /// <param name="targetState"></param>
    /// <param name="isSuccessed"></param>
    /// <returns> Fianl Troop State </returns>
    public TroopState TryUpdateState(TroopState targetState, out bool isSuccessed)
    {
        if (troopState == targetState)
        {
            isSuccessed = false;
            return troopState;
        }

        if (targetState == TroopState.CHASE || targetState == TroopState.BATTLE)
        {
            isSuccessed = false;
            return troopState;
        }

        TroopState originState = troopState;
        TroopState resultState = CheckTransition(targetState);

        isSuccessed = resultState != originState;
        if (isSuccessed == true)
        {
            troopState = resultState;
            onPostTroopStateChanged.Invoke(troopState);
        }
        return resultState;
    }

    public TroopState CheckTransition(TroopState originState)
    {
        TroopState targetState = originState;
        if (originState == TroopState.IDLE)
        {
            // TODO Dectect Logic
            bool checkEnemyDetection = false;
            if (checkEnemyDetection == true) targetState = TroopState.CHASE;
            else targetState = TroopState.IDLE;
        }


        if (originState != targetState)
            return CheckTransition(targetState);

        // targetState == originState
        return targetState;
    }

    /// <summary>
    /// 리더 미니언 갱신
    /// </summary>
    /// <returns> Selected leader minion</returns>
    public Minion UpdateLeaderMinion()
    {
        //TODO 리더 미니언을 셀렉하는 방식을 고안한 후에 변경해야함.
        leaderMinion = minions.First();
        return leaderMinion;
    }

    /// <summary>
    /// 미니언 생성, 초기화 
    /// </summary>
    /// <returns> Generated minion</returns>
    public Minion AddMinion()
    {
        //TODO 미니언 추가 시스템을 구축한 후에 변경해야함.
        Minion minion = null;
        onPostMinionAdded.Invoke(minion);

        return minion;
    }


}
