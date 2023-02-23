using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Netcode;

public class GameSessionSynchronizer : NetworkBehaviour
{
    public float replicationRange = 100f;
    private HashSet<ClientIDCombinationContainer> clientIDCombationSet = new HashSet<ClientIDCombinationContainer>();
    private HashSet<ulong> clinetIDSet = new HashSet<ulong>();

    private Dictionary<PlayerData, Dictionary<PlayerData, bool>> replicationFlagTargetTablePerPlayer =
        new Dictionary<PlayerData, Dictionary<PlayerData, bool>>();

    public bool AddClientID(PlayerData playerData)
    {
        ulong clientID = playerData.ClientID;
        if (clinetIDSet.Contains(clientID)) return false;

        var newCombinationList = new List<ClientIDCombinationContainer>();
        foreach (var idCombinationContainer in clientIDCombationSet)
        {
            var predefinedCombination = idCombinationContainer.ClientIDCombination;
            var combination = new ulong[predefinedCombination.Length + 1];
            predefinedCombination.CopyTo(combination, 0);
            combination[combination.Length - 1] = clientID;
            newCombinationList.Add(new ClientIDCombinationContainer(combination));
        }

        for (int i = 0; i < newCombinationList.Count; i++) clientIDCombationSet.Add(newCombinationList[i]);
        clinetIDSet.Add(clientID);

        return true;
    }

    private void AddNewPlayerToReplcationTarget(PlayerData playerData)
    {
        if (replicationFlagTargetTablePerPlayer.ContainsKey(playerData)) return;

        foreach (var flagDic in replicationFlagTargetTablePerPlayer)
            flagDic.Value.Add(playerData, true);
        
        replicationFlagTargetTablePerPlayer.Add(playerData,new Dictionary<PlayerData, bool>());

        var newFlagDic = replicationFlagTargetTablePerPlayer[playerData];
        foreach (var replcationTable in replicationFlagTargetTablePerPlayer)
            if(replcationTable.Key != playerData) newFlagDic.Add(replcationTable.Key,true);
    }

    private void RefreshReplicationTable()
    {
        foreach (var replicationFlagTable in replicationFlagTargetTablePerPlayer)
        {
            var myPlayerData = replicationFlagTable.Key;
            //TODO: Get leader minion
            var standardPos = myPlayerData.MinionInstanceList[0].transform.position;
            foreach (var flagDic in replicationFlagTable.Value)
            {
                var otherPlayerData = flagDic.Key;
                //TODO: Get leader minion
                var otherPlayerPos = otherPlayerData.MinionInstanceList[0].transform.position;
                var isWithinRagne = Vector3.Distance(standardPos, otherPlayerPos) < replicationRange;
                replicationFlagTable.Value[flagDic.Key] = isWithinRagne;
            }
        }
    }

    private IEnumerator SyncMinionInformationFromServer()
    {

        yield break;
        
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void ReceiveMinionInformations(ClientRpcParams clientRPCParams)
    {
            

    }

    private class ClientIDCombinationContainer
    {
        private ulong[] clientIDCombination;
        private readonly int hashCode;
        private HashSet<ulong> clientIDSet = new HashSet<ulong>();

        public ulong[] ClientIDCombination => clientIDCombination;

        private ClientIDCombinationContainer()
        {
        }

        public ClientIDCombinationContainer(ulong[] clientIDCombination)
        {
            this.clientIDCombination = clientIDCombination;

            hashCode = 0;
            foreach (var id in clientIDCombination)
            {
                clientIDSet.Add(id);
                hashCode += Convert.ToInt32(id);
            }
        }

        public override int GetHashCode() => hashCode;

        public override bool Equals(object obj)
        {
            var containter = obj as ClientIDCombinationContainer;
            int duplicatedCount = 0;
            var idCombination = containter.clientIDCombination;
            for (int i = 0; i <idCombination.Length; i++)
                if (clientIDSet.Contains(idCombination[i])) duplicatedCount++;

            return duplicatedCount == clientIDCombination.Length;
        }
    }
}

public class GameSessionSynchronizingInfo
{
    
}




