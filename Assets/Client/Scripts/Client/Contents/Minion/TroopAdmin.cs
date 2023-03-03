using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
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

[RequireComponent(typeof(NetworkObject))]
public class TroopAdmin : NetworkBehaviour
{
    [ShowInInspector, ReadOnly] public List<Minion> minions { get; private set; }
    [ShowInInspector, ReadOnly] public Minion leaderMinion { get; private set; }
    [ShowInInspector, ReadOnly] public TroopState troopState { get; private set; }

    [ShowInInspector, ReadOnly] public int[] minionHandDeck { get; private set; }

    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isOwner;

    public bool IsPlayer { get { return isPlayer; } }
    public bool IsOwner { get { return isOwner; } }

    [HideInInspector] public UnityEvent<Minion> onPostMinionAdded;
    [HideInInspector] public UnityEvent<TroopState> onPostTroopUpdated;
    [HideInInspector] public UnityEvent<TroopState> onPostTroopStateChanged;

    void Awake()
    {
        troopState = TroopState.IDLE;
        minions = new List<Minion>();
        leaderMinion = null;
        minionHandDeck = new int[4];
    }

    void Start()
    {
        //Test
        for (int i = 0; i < transform.childCount; i++)
        {
            minions.Add(transform.GetChild(i).GetComponent<Minion>());
        }
        if (minions.Count > 0)
        {
            leaderMinion = minions[0];
        }

        ChangeState(TroopState.IDLE);
        foreach (var each in minions)
        {
            each.obstacle.enabled = false;
            each.agent.enabled = true;
        }

        if (isPlayer)
        {
            CameraManager.Instance.followingTarget = leaderMinion.transform;
        }
    }

    private void Update()
    {

        if (minions.Count > 0)
        {
            if (isOwner) OwnerUpdate();
            else BotUpdate();

            onPostTroopUpdated.Invoke(troopState);

            TroopState newState = CheckTransition(troopState);
            if (troopState != newState) ChangeState(newState);
        }
    }

    private void BotUpdate()
    {
        if (troopState == TroopState.IDLE)
        {
        }
        else if (troopState == TroopState.MOVE)
        {
            if (minions.Any((Minion minion) => (minion.agent.transform.position - minion.agent.destination).magnitude <= minion.agent.stoppingDistance))
            {
                Vector3 newAroundDestination = SessionManager.instance.map.GetRandomPosition();
                while ((leaderMinion.agent.destination - newAroundDestination).magnitude < 25) newAroundDestination = SessionManager.instance.map.GetRandomPosition();

                foreach (var each in minions)
                {
                    each.agent.stoppingDistance = each.agent.radius + 0.5f;
                    NavMeshPath path = new NavMeshPath();
                    //   NavMesh.CalculatePath(each.agent.transform.position, aroundDestination, 0, path);
                    each.agent.CalculatePath(newAroundDestination, path);
                    each.agent.SetPath(path);
                }
            }
        }
    }
    private void OwnerUpdate()
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
            foreach (var each in minions)
            {
                each.agent.stoppingDistance = 0;
                each.agent.SetDestination(each.transform.position + InputManager.instance.dragAxis);
            }
        }
        else if (troopState == TroopState.CHASE)
        {
            foreach (var each in minions)
            {
                each.agent.stoppingDistance = 0;

                // 찾은 적이 있을경우 해당 적을향해, 없을경우 리더 미니언을 쫒아가면서 적을 찾음.
                if (each.recognizedEnemies.Count > 0) each.agent.SetDestination(each.recognizedEnemies[0].transform.position);
                else each.agent.SetDestination(leaderMinion.recognizedEnemies[0].transform.position);
            }
        }
        else if (troopState == TroopState.BATTLE)
        {
            foreach (var each in minions)
            {
                each.agent.stoppingDistance = 0;

                // 찾은 적이 있을경우 해당 적을향해, 없을경우 리더 미니언을 쫒아가면서 적을 찾음.
                if (each.recognizedEnemies.Count > 0) each.agent.SetDestination(each.recognizedEnemies[0].transform.position);
                else each.agent.SetDestination(leaderMinion.recognizedEnemies[0].transform.position);
            }
        }
    }

    void ChangeState(TroopState newState)
    {
        TroopState oldState = troopState;
        troopState = newState;
        onPostTroopStateChanged.Invoke(troopState);

        // 만약 전투상태일 때 적과 거리가 멀어질경우 배틀 AI는 중단
        if (oldState == TroopState.BATTLE)
        {
            foreach (var minion in minions)
            {
                minion.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, minion.Stat.MyBattleAbility.AttackBehaviour);
                minion.obstacle.enabled = false;
                minion.agent.enabled = true;
            }
        }

        // 만약 전투상태가 아닐 때 적과 거리가 가까워질 경우 배틀 AI 시작/재개
        if (newState == TroopState.BATTLE)
        {
            foreach (var minion in minions)
            {
                minion.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, minion.Stat.MyBattleAbility.AttackBehaviour);
                minion.obstacle.enabled = true;
                minion.agent.enabled = false;
            }

        }



        if (newState == TroopState.MOVE)
        {
            if (!isPlayer)
            {
                Vector3 randomPosition = SessionManager.instance.map.GetRandomPosition();
                foreach (var each in minions)
                {
                    each.agent.stoppingDistance = each.agent.radius + 0.5f;
                    each.agent.SetDestination(randomPosition);
                }
            }
        }
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
            ChangeState(resultState);
        }
        return resultState;
    }

    public TroopState CheckTransition(TroopState originState)
    {
        TroopState targetState = originState;
        if (originState == TroopState.IDLE)
        {
            if (isOwner)
            {
                bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count > 0;
                // TODO Dectect Logic

                if (checkEnemyDetection == true) targetState = TroopState.CHASE;

                if (InputManager.instance.dragAxis.magnitude > 0)
                    targetState = TroopState.MOVE;
            }
            else
            {
                targetState = TroopState.MOVE;
            }

        }
        else if (originState == TroopState.MOVE)
        {
            if (isOwner)
            {
                if (InputManager.instance.dragAxis.magnitude == 0)
                    targetState = TroopState.IDLE;
            }
            else
            {
                bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count > 0;
                // TODO Dectect Logic

                if (checkEnemyDetection == true) targetState = TroopState.CHASE;
            }
        }
        else if (originState == TroopState.CHASE)
        {
            if (isOwner)
            {
                if (InputManager.instance.dragAxis.magnitude > 0)
                    targetState = TroopState.MOVE;
            }
            else
            {
                bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count == 0;
                if (checkEnemyDetection == true) targetState = TroopState.IDLE;
            }
        }
        else if (originState == TroopState.BATTLE)
        {
            if (isOwner)
            {
                if (InputManager.instance.dragAxis.magnitude > 0)
                    targetState = TroopState.MOVE;
            }
            else
            {
                bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count == 0;
                if (checkEnemyDetection == true) targetState = TroopState.IDLE;
            }
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
