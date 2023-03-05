
using System.Threading;
using Cysharp.Threading.Tasks;

public class MinionStateBattle : MinionState
{
    public MinionStateBattle(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
        if (MyInputManager.DragAxis.magnitude > 0)
            return new MinionStateMove(Owner);

        if (Owner.chaseTarget != null)
        {
            if ((Owner.agent.transform.position - Owner.chaseTarget.agent.destination).magnitude > Owner.agent.stoppingDistance)
                return new MinionStateChase(Owner);
        }

        return this;
    }

    public override async UniTask EnterState()
    {
        //  owner.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, owner.Stat.MyBattleAbility.AttackBehaviour);

        Owner.obstacle.enabled = false;
        Owner.agent.enabled = true;
        Owner.agent.avoidancePriority = 40;
    }

    public override async UniTask ExitState()
    {
        //  owner.Stat.MyBattleAbility.CombatAI.SetActiveAI(true, owner.Stat.MyBattleAbility.AttackBehaviour);
        Owner.obstacle.enabled = false;
        Owner.agent.enabled = true;
        Owner.agent.avoidancePriority = 50;

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

            Owner.Attack(Owner.chaseTarget);
            await UniTask.NextFrame(cancellationToken);
        }
    }

    private bool WaitForAttackDealy()
    {
        return Owner.Stat.MyAttackBehaviour.IsAttackable;
    }
}
