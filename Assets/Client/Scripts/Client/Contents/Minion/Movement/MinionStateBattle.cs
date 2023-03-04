
using System.Threading;
using Cysharp.Threading.Tasks;

public class MinionStateBattle : MinionState
{
    public MinionStateBattle(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
            if (InputManager.instance.dragAxis.magnitude > 0)
                return new MinionStateMove(owner);

        if (owner.chaseTarget != null)
        {
            if ((owner.agent.transform.position - owner.chaseTarget.agent.destination).magnitude > owner.agent.stoppingDistance)
                return new MinionStateChase(owner);
        }

        return this;
    }

    public override void EnterState()
    {
        //  owner.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, owner.Stat.MyBattleAbility.AttackBehaviour);

        owner.obstacle.enabled = false;
        owner.agent.enabled = true;
        owner.agent.avoidancePriority = 40;
    }

    public override void ExitState()
    {
        //  owner.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, owner.Stat.MyBattleAbility.AttackBehaviour);
        owner.obstacle.enabled = false;
        owner.agent.enabled = true;
        owner.agent.avoidancePriority = 50;

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

            }
            else
            {

            }
            await UniTask.NextFrame();
        }
    }
}
