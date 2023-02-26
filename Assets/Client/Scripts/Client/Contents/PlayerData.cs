using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;
using Unity.Netcode;

[Serializable]
public class PlayerData : IMinionDeployable,  INetworkSerializable
{
    private List<MinionData> minionDeck;
    private List<Minion> minionInstanceList;
    private PlayerSession playerSession;
    private ulong clientID;

    public IReadOnlyList<MinionData> MinionDeck => minionDeck;

    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;

    public PlayerSession PlayerSession => playerSession;
    public ulong ClientID => clientID;

    private PlayerData()
    {
    }

    public PlayerData(List<MinionData> deck, PlayerSession playerSession )
    {
        minionDeck = deck;
        this.playerSession = playerSession;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        throw new NotImplementedException();
    }
}
