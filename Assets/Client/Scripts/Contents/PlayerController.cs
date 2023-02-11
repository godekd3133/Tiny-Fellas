using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TroopAdmin troopAdmin;

    void Start()
    {
        troopAdmin.onPostTroopUpdated.AddListener(OnPostTroopUpdate);
        troopAdmin.onPostTroopStateChanged.AddListener(OnPostTroopStateChange);
    }
    private void OnPostTroopStateChange(TroopState state)
    {
        Debug.Log($"[PlayerController] Changed State Into {state}");
    }
    private void OnPostTroopUpdate(TroopState state)
    {
        if (state == TroopState.IDLE)
        {
            if (InputManager.instance.dragAxis.magnitude > 0)
                troopAdmin.TryUpdateState(TroopState.MOVE, out _);
        }
        if (state == TroopState.MOVE)
        {
            if (InputManager.instance.dragAxis.magnitude == 0)
                troopAdmin.TryUpdateState(TroopState.IDLE, out _);
            else
            {
                foreach (var each in troopAdmin.minions)
                {
                    each.agent.stoppingDistance = 0;
                    each.agent.SetDestination(each.transform.position + InputManager.instance.dragAxis);
                }
            }
        }
    }
}
