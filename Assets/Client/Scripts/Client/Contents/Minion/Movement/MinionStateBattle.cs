
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
            if ((Owner.chaseTarget.transform.position - Owner.transform.position).magnitude <= Owner.Stat.MyBattleAbility[EStatName.ATTACK_RAGNE].CurrentValue)
            {

                return new MinionStateChase(Owner);
            }
        }
        else
        {
            return new MinionStateIdle(Owner);

        }


        return this;
    }

    public override async UniTask EnterState()
    {
        Owner.obstacle.enabled = false;
        Owner.agent.enabled = true;
        Owner.agent.avoidancePriority = 40;
    }

    public override async UniTask ExitState()
    {
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

            if (!Owner.Stat.MyAttackBehaviour.IsOnAttacking)
            {
                Owner.Attack(Owner.chaseTarget);
            }
            await UniTask.NextFrame(cancellationToken);
        }

    }

    private bool WaitForAttackDealy()
    {
        return Owner.Stat.MyAttackBehaviour.IsAttackable;
    }
}
