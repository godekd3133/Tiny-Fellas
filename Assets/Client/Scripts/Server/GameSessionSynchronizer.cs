using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Netcode;

public class GameSessionSynchronizer : MonoWeakSingletonPerScene<GameSessionSynchronizer>
{
    public float replicationRange = 100f;
    private HashSet<ClientIDCombinationContainer> clientIDCombationSet;
    
    private IEnumerator SyncMinionInformationFromServer()
    {
        var gameSession = GameSessionInstance.GetInstanceOfScene(gameObject.scene);
        var playerDataList = gameSession.PlayerDataList;

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




