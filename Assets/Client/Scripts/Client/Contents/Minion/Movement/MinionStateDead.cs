
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Unity.Netcode;

public class MinionStateDead : MinionState
{
    bool isRevived;

    public MinionStateDead(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
        if (!MyTroopAdmin.IsInBattle())
        {
            isRevived = true;
            return new MinionStateIdle(Owner);
        }
        return this;
    }

    public override async UniTask EnterState()
    {
        isRevived = false;
    }

    public override async UniTask ExitState()
    {
        if (isRevived == true)
        {
            //TODO 체력회복
        }
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
            await UniTask.NextFrame();
        }
    }
}
