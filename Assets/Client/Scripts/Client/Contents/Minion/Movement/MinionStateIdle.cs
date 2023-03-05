
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;

public class MinionStateIdle : MinionState
{

    public MinionStateIdle(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
            if (MyInputManager.DragAxis.magnitude > 0)
                return new MinionStateMove(Owner);

            Minion leaderMinion = Owner.troopAdmin.leaderMinion;

            bool checkEnemyDetection = leaderMinion.recognizedEnemies.Count > 0;

            if (checkEnemyDetection == true)
                return new MinionStateChase(Owner);


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

            Minion leaderMinion = MyTroopAdmin.leaderMinion;
            if (Owner.agent.destination != MyTroopAdmin.leaderMinion.transform.position)
            {
                Owner.agent.stoppingDistance = 2.5f;
                Owner.agent.SetDestination(leaderMinion.transform.position);
            }

            await UniTask.NextFrame();
        }
    }
}
