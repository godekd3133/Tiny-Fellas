using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(GameSessionInstance))]
public class GameSessionSynchronizer : NetworkBehaviour
{
    private Dictionary<int, Vector3[]> cachedVector3ArrPerSize = new Dictionary<int, Vector3[]>();
    private Dictionary<int, Quaternion[]> cachedQuaternionArrPerSize = new Dictionary<int, Quaternion[]>();

    private void Update()
    {
        SynchronizeAllMinionTransform();
    }

    private void SynchronizeAllMinionTransform()
    {
        var count = GameSessionInstance.Instance.PlayerDataList.Count;
        for (int i = 0; i < count; i++)
        {
            var playerData = GameSessionInstance.Instance.PlayerDataList[i];
            var minionInstanceList = playerData.MinionInstanceList;
            int minionCount = minionInstanceList.Count;

            Vector3[] positionArr;
            Quaternion[] rotationArr;

            if (cachedVector3ArrPerSize.ContainsKey(minionCount)) positionArr = cachedVector3ArrPerSize[minionCount];
            else
            {
                positionArr = new Vector3[minionCount];
                cachedVector3ArrPerSize.Add(minionCount, positionArr);
            }
            
            if (cachedQuaternionArrPerSize.ContainsKey(minionCount)) rotationArr = cachedQuaternionArrPerSize[minionCount];
            else
            {
                rotationArr = new Quaternion[minionCount];
                cachedQuaternionArrPerSize.Add(minionCount, rotationArr);
            }

            for (int j = 0; j < minionCount; j++)
            {
                var minion = minionInstanceList[j];
                positionArr[j] = minion.transform.position;
                rotationArr[j] = minion.transform.rotation;
            }

            SynchronizeMinionTransforms_ClientRPC(playerData.ClientID, positionArr, rotationArr);
        }
    }

    [ClientRpc]
    private void SynchronizeMinionTransforms_ClientRPC(ulong clientID, Vector3[] positionList,Quaternion[] rotationList)
    {
        var minionInstanceList = GameSessionInstance.Instance.PlayerDataByClientID[clientID].MinionInstanceList;

        for (int i = 0; i < positionList.Length; i++)
        {
            minionInstanceList[i].transform.position = positionList[i];
            minionInstanceList[i].transform.rotation = rotationList[i];
        }
    }
}

 /*private HashSet<ClientIDCombinationContainer> clientIDCombationSet = new HashSet<ClientIDCombinationContainer>();
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

        replicationFlagTargetTablePerPlayer.Add(playerData, new Dictionary<PlayerData, bool>());

        var newFlagDic = replicationFlagTargetTablePerPlayer[playerData];
        foreach (var replcationTable in replicationFlagTargetTablePerPlayer)
            if (replcationTable.Key != playerData) newFlagDic.Add(replcationTable.Key, true);
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
    }*/

    /*private class ClientIDCombinationContainer
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
            for (int i = 0; i < idCombination.Length; i++)
                if (clientIDSet.Contains(idCombination[i])) duplicatedCount++;

            return duplicatedCount == clientIDCombination.Length;
        }
    }*/



