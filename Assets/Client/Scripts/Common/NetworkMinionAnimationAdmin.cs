using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkMinionAnimationAdmin : NetworkBehaviourSingleton<NetworkMinionAnimationAdmin>
{
    [ClientRpc]
    public void PlayAnimation_ClientRPC(ulong clientID, int minionIndexInInstacneList,  string animparam, int value )
    {
        var animator = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList[minionIndexInInstacneList]
            .GetComponent<Animator>();
        
        animator.SetInteger(animparam,value);
    }
    
    [ClientRpc]
    public void PlayAnimation_ClientRPC(ulong clientID, int minionIndexInInstacneList,  string animparam, float value )
    {
        var animator = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList[minionIndexInInstacneList]
            .GetComponent<Animator>();
        
        animator.SetFloat(animparam,value);
    }
    
    [ClientRpc]
    public void PlayAnimation_ClientRPC(ulong clientID, int minionIndexInInstacneList,  string animparam, bool value )
    {
        var animator = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList[minionIndexInInstacneList]
            .GetComponent<Animator>();
        
        animator.SetBool(animparam,value);
    }
    
    [ClientRpc]
    public void PlayAnimation_ClientRPC(ulong clientID, int minionIndexInInstacneList,  string animparam )
    {
        var animator = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList[minionIndexInInstacneList]
            .GetComponent<Animator>();
        
        animator.SetTrigger(animparam);
    }
    
    [ClientRpc]
    public void PlayAnimationState_ClientRPC(ulong clientID, int minionIndexInInstacneList,  string stateName )
    {
        var animator = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList[minionIndexInInstacneList]
            .GetComponent<Animator>();
        
        animator.Play(stateName);
    }
}
