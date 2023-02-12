using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amazon.CloudFormation.Model;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public TroopAdmin troopAdmin;

    [ReadOnly, ShowInInspector] Vector3 aroundDestination;

    void Start()
    {
        troopAdmin.onPostTroopUpdated.AddListener(OnPostTroopUpdate);
        troopAdmin.onPostTroopStateChanged.AddListener(OnPostTroopStateChange);

    }
    private void OnPostTroopStateChange(TroopState state)
    {
        Debug.Log($"[BotController] Changed State Into {state}");
        if (state == TroopState.MOVE)
        {
            aroundDestination = FindRandomDestination();
            foreach (var each in troopAdmin.minions)
            {
                each.agent.stoppingDistance = each.agent.radius + 0.5f;
                each.agent.SetDestination(aroundDestination);
            }
            Debug.Log(aroundDestination);
        }
    }
    private void OnPostTroopUpdate(TroopState state)
    {
        if (state == TroopState.IDLE)
        {
            troopAdmin.TryUpdateState(TroopState.MOVE, out _);

        }
        else if (state == TroopState.MOVE)
        {
            //TODO Detect Logic
            bool detectedEnemy = false;


            if (troopAdmin.minions.Any((Minion minion) => (minion.agent.transform.position - aroundDestination).magnitude <= minion.agent.stoppingDistance))
            {
                Vector3 newAroundDestination = FindRandomDestination();
                while ((aroundDestination - newAroundDestination).magnitude < 25) newAroundDestination = FindRandomDestination();

                aroundDestination = newAroundDestination;
                foreach (var each in troopAdmin.minions)
                {
                    each.agent.stoppingDistance = each.agent.radius + 0.5f;
                    NavMeshPath path = new NavMeshPath();
                    //   NavMesh.CalculatePath(each.agent.transform.position, aroundDestination, 0, path);
                    each.agent.CalculatePath(aroundDestination, path);
                    each.agent.SetPath(path);
                }
                aroundDestination = troopAdmin.leaderMinion.agent.destination;
                Debug.Log(aroundDestination);
            }
        }
    }

    private Vector3 FindRandomDestination()
    {
        Vector3 mapSize = SessionManager.instance.map.GetMapSize();
        Vector3 randomPosition;
        randomPosition.x = ((float)Random.Range(0, 100) / 100f - 0.5f) * mapSize.x;
        randomPosition.y = 0;
        randomPosition.z = ((float)Random.Range(0, 100) / 100f - 0.5f) * mapSize.z;
        return randomPosition;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (var each in troopAdmin.minions)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(each.transform.position, aroundDestination);
            }
        }
    }


}
