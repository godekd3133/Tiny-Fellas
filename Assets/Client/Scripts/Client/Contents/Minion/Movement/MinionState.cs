
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public abstract class MinionState
{
    protected Minion owner;


    public UnityEvent onPostStateUpdated;

    public bool enabled;

    public MinionState(Minion owner)
    {
        this.owner = owner;
        enabled = true;
    }

    public abstract MinionState CheckTransition();
    public abstract void EnterState();
    public abstract UniTask UpdateState(CancellationToken cancellationToken);
    public abstract void ExitState();

}
