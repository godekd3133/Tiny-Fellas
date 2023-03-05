
using System.Threading;
using Cysharp.Threading.Tasks;

public class MinionStateMove : MinionState
{
    public MinionStateMove(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
            if (MyInputManager.DragAxis.magnitude == 0f)
                return new MinionStateIdle(Owner);
            else
            {
                bool checkEnemyDetection = MyTroopAdmin.RecognizedEnemyMinionList.Count > 0;
                if (checkEnemyDetection == true) return new MinionStateChase(Owner);
            }

            return this;
    }

    public override void EnterState()
    {
            /*Vector3 randomPosition = SessionManager.instance.map.GetRandomPosition();

            Owner.agent.stoppingDistance = owner.agent.radius + 0.5f;
            Owner.agent.SetDestination(randomPosition);*/
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

            Owner.agent.stoppingDistance = 0.05f;
            Owner.agent.SetDestination(Owner.transform.position + MyInputManager.DragAxis);
            /*else
            {
                if ((owner.agent.transform.position - owner.agent.destination).magnitude <= owner.agent.stoppingDistance)
                {
                    Vector3 newAroundDestination = SessionManager.instance.map.GetRandomPosition();
                    while ((owner.agent.destination - newAroundDestination).magnitude < 25) newAroundDestination = SessionManager.instance.map.GetRandomPosition();

                    // foreach (var each in NetworkManager)
                    // {
                    //     each.agent.stoppingDistance = each.agent.radius + 0.5f;
                    //     UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                    //     //   NavMesh.CalculatePath(each.agent.transform.position, aroundDestination, 0, path);
                    //     each.agent.CalculatePath(newAroundDestination, path);
                    //     each.agent.SetPath(path);
                    // }
                }
            }*/
            await UniTask.NextFrame();
        }
    }
}
