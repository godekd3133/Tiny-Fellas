
using System.Threading;
using Cysharp.Threading.Tasks;

public class MinionStateChase : MinionState
{
    public MinionStateChase(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
            if (MyInputManager.DragAxis.magnitude > 0)
                return new MinionStateMove(Owner);


        bool checkEnemyDetection = Owner.troopAdmin.leaderMinion.recognizedEnemies.Count > 0;
        if (checkEnemyDetection == false)
            return new MinionStateIdle(Owner);

        if (Owner.chaseTarget != null)
            if ((Owner.agent.transform.position - Owner.chaseTarget.agent.destination).magnitude <= Owner.agent.stoppingDistance)
                return new MinionStateBattle(Owner);

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
            }            //owner.Stat.MyBattleAbility.StatMap[EStatName.ATTACK_RAGNE].CurrentValue ?? 
            Owner.agent.stoppingDistance = 2f - 0.15f;
            // 찾은 적이 있을경우 해당 적을향해, 없을경우 리더 미니언을 쫒아가면서 적을 찾음.
            if (Owner.recognizedEnemies.Count > 0)
            {
                Owner.agent.SetDestination(Owner.recognizedEnemies[0].transform.position);
                Owner.chaseTarget = Owner.recognizedEnemies[0];
            }
            else
            {
                Owner.agent.SetDestination(Owner.troopAdmin.leaderMinion.recognizedEnemies[0].transform.position);
                Owner.chaseTarget = Owner.troopAdmin.leaderMinion.recognizedEnemies[0];
            }

            await UniTask.NextFrame();
        }
    }
}
