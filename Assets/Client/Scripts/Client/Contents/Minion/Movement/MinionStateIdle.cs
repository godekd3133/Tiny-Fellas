
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine.Events;

public class MinionStateIdle : MinionState
{

    public MinionStateIdle(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
        if (owner.troopAdmin.IsOwner)
        {
            if (InputManager.instance.dragAxis.magnitude > 0)
                return new MinionStateMove(owner);

            Minion leaderMinion = owner.troopAdmin.leaderMinion;

            bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count > 0;

            if (checkEnemyDetection == true)
                return new MinionStateChase(owner);


        }
        else
        {
            return new MinionStateMove(owner);
        }

        return this;
    }

    public override void EnterState()
    {
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
                if (!owner.isLeader)
                {
                    Minion leaderMinion = owner.troopAdmin.leaderMinion;
                    if (owner.agent.destination != owner.troopAdmin.leaderMinion.transform.position)
                    {
                        owner.agent.stoppingDistance = 2.5f;
                        owner.agent.SetDestination(leaderMinion.transform.position);
                    }
                }
            }
            await UniTask.NextFrame();
        }
    }
}
