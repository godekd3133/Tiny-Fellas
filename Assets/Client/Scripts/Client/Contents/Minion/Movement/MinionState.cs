
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;
using UnityEngine.Events;

public abstract class MinionState
{
    private Minion owner;
    private InputManager inputManager;
    private TroopAdmin troopAdmin;

    public Minion Owner => owner;
    public InputManager MyInputManager => inputManager;
    public TroopAdmin MyTroopAdmin => troopAdmin;


    public UnityEvent onPostStateUpdated;

    public bool enabled;

    private MinionState(){}
    
    public MinionState(Minion owner)
    {
        this.owner = owner;
        enabled = true;
         troopAdmin = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(owner.OwnerClientId).GetComponent<TroopAdmin>();
        inputManager = troopAdmin.GetComponentInChildren<InputManager>();
    }

    public abstract MinionState CheckTransition();
    public abstract void EnterState();
    public abstract UniTask UpdateState(CancellationToken cancellationToken);
    public abstract void ExitState();

}
