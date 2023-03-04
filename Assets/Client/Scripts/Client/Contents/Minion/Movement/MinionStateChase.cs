
using System.Threading;
using Cysharp.Threading.Tasks;

public class MinionStateChase : MinionState
{
    public MinionStateChase(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
            if (InputManager.instance.dragAxis.magnitude > 0)
                return new MinionStateMove(owner);


        bool checkEnemyDetection = owner.troopAdmin.leaderMinion.recognizedEnemies.Count > 0;
        if (checkEnemyDetection == false)
            return new MinionStateIdle(owner);

        if (owner.chaseTarget != null)
            if ((owner.agent.transform.position - owner.chaseTarget.agent.destination).magnitude <= owner.agent.stoppingDistance)
                return new MinionStateBattle(owner);

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
            owner.agent.stoppingDistance = 2f - 0.15f;
            // 찾은 적이 있을경우 해당 적을향해, 없을경우 리더 미니언을 쫒아가면서 적을 찾음.
            if (owner.recognizedEnemies.Count > 0)
            {
                owner.agent.SetDestination(owner.recognizedEnemies[0].transform.position);
                owner.chaseTarget = owner.recognizedEnemies[0];
            }
            else
            {
                owner.agent.SetDestination(owner.troopAdmin.leaderMinion.recognizedEnemies[0].transform.position);
                owner.chaseTarget = owner.troopAdmin.leaderMinion.recognizedEnemies[0];
            }

            await UniTask.NextFrame();
        }
    }
}
