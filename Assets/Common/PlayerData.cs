using System;
using System.Collections.Generic;
using Amazon.GameLift.Model;

[Serializable]
public class PlayerData : IMinionDeployable
{
    private List<Minion> minionDeck;
    private List<Minion> minionInstanceList;
    private PlayerSession playerSession;

    public IReadOnlyList<Minion> MinionDeck => minionDeck;

    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;

    public PlayerSession PlayerSession => playerSession;
}
