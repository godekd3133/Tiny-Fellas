using System;
using System.Collections;
using System.Collections.Generic;
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
        
    }

    private void RefreshReplicationTable()
    {
        
    }

    private IEnumerator SyncMinionInformationFromServer()
    {
        var gameSession = GameSessionInstance.Singleton;

        yield break;
        
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void ReceiveMinionInformations(ClientRpcParams clientRPCParams)
    {
            

    }

    public List<PlayerData> GetPlayerDatasWithinReplicationRagne(PlayerData standardPlayer, List<PlayerData> playerDatas)
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




