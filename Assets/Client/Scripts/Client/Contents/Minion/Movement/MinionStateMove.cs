
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MinionStateMove : MinionState
{
    public MinionStateMove(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
        if (owner.troopAdmin.IsOwner)
        {
            if (InputManager.instance.dragAxis.magnitude == 0f)
                return new MinionStateIdle(owner);
        }
        else
        {
            bool checkEnemyDetection = owner.troopAdmin.leaderMinion.recognizedEnemies.Count > 0;
            if (checkEnemyDetection == true) return new MinionStateChase(owner);
        }
        return this;


    }

    public override void EnterState()
    {
        if (!owner.troopAdmin.IsOwner)
        {
            Vector3 randomPosition = SessionManager.instance.map.GetRandomPosition();

            owner.agent.stoppingDistance = owner.agent.radius + 0.5f;
            owner.agent.SetDestination(randomPosition);
        }
    }

    public override void ExitState()
    {
    }



    public override async UniTask UpdateState(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                enabled = false;
                break;
            }

            if (owner.troopAdmin.IsOwner)
            {
                owner.agent.stoppingDistance = 0;
                owner.agent.SetDestination(owner.transform.position + InputManager.instance.dragAxis);
            }
            else
            {
                if ((owner.agent.transform.position - owner.agent.destination).magnitude <= owner.agent.stoppingDistance)
                {
                    Vector3 newAroundDestination = SessionManager.instance.map.GetRandomPosition();
                    while ((owner.agent.destination - newAroundDestination).magnitude < 25) newAroundDestination = SessionManager.instance.map.GetRandomPosition();

                    foreach (var each in owner.troopAdmin.minions)
                    {
                        each.agent.stoppingDistance = each.agent.radius + 0.5f;
                        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                        //   NavMesh.CalculatePath(each.agent.transform.position, aroundDestination, 0, path);
                        each.agent.CalculatePath(newAroundDestination, path);
                        each.agent.SetPath(path);
                    }
                }
            }
            await UniTask.NextFrame();
        }
    }
}
