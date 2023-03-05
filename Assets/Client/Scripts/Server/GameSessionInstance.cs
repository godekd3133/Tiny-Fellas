using System;
using System.Collections.Generic;
using System.Threading;
using Amazon.GameLift.Model;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


[RequireComponent(typeof(NetworkObject))]
public class GameSessionInstance : NetworkBehaviourSingleton<GameSessionInstance>
#if UNITY_SERVER || UNITY_EDITOR
    , IMinionDeployable
#endif
{
    //========================================================================================
    //Callback Functions
    //========================================================================================
    public UnityEvent<PlayerData> onPostNewPlayerConnect;   // Server , Client
    public UnityEvent<PlayerData> onPostPlayerDisConnect;   // Server , Client
    public UnityAction onGameStart;                         // Server , Client
    //========================================================================================

    //========================================================================================
    //Game Room Properties
    //========================================================================================
    [Space(20), Header("Game Room Properties")]
    // 유저 매칭 최대 대기시간, 해당 시간을 넘기도록 유저가 모이지 않으면 AI로 채우고 시작
    [SerializeField] private float maxPreparationTime;
    public float MaxPreparationTime { get { return maxPreparationTime; } }

    // 세션에 접속할 수 있는 최대 유저 수
    public int MaxUserCount => 10;
    //========================================================================================

    //========================================================================================
    //Runtime Local Variables
    //========================================================================================
    /// Server Only

    /// Client Only

    /// Mixed
    [Space(20), Header("Runtime Only Variables")]
    /// 유저 매칭 최대 대기시간, 해당 시간을 넘기도록 유저가 모이지 않으면 AI로 채우고 시작
    [SerializeField] private bool isGameRunning;
    public bool IsGameRunning { get { return isGameRunning; } }
    [SerializeField] private bool isSessionRunning;
    public bool IsSessionRunning { get { return isSessionRunning; } }

    [SerializeField] private PlayerHandDeck handDeck;
    public PlayerHandDeck HandDeck { get { return handDeck; } }
    //========================================================================================

    //========================================================================================
    //Runtime Sync Properties
    //========================================================================================
    public NetworkVariable<float> SessionStartTime { get; private set; }
    public NetworkVariable<float> SessionTime { get; private set; }
    public NetworkVariable<float> GameTime { get; private set; }
    public NetworkVariable<float> GameStartTime { get; private set; }
    //========================================================================================


#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<PlayerData> playerDataList = new List<PlayerData>();
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<MinionData> minionDeck;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<Minion> minionInstanceList;
    private Dictionary<ulong, PlayerData> playerDataByClientID = new Dictionary<ulong, PlayerData>();
    private GameSession gameSession;

#if UNITY_SERVER || UNITY_EDITOR
    public IReadOnlyList<MinionData> MinionDeck => minionDeck;
    public IReadOnlyList<Minion> MinionInstanceList => minionInstanceList;
#endif
    public IReadOnlyList<PlayerData> PlayerDataList => playerDataList;

    public IReadOnlyDictionary<ulong, PlayerData> PlayerDataByClientID
    {
        get => playerDataByClientID;
    }

    private void Awake()
    {
        Initialize();
#if !UNITY_SERVER

#endif
    }

    private void Initialize()
    {
        isSessionRunning = false;
        isGameRunning = false;

        SessionStartTime = new NetworkVariable<float>(0f);
        SessionTime = new NetworkVariable<float>(0f);
        GameStartTime = new NetworkVariable<float>(0f);
        GameTime = new NetworkVariable<float>(0f);

        AWSFleetManager.Instance.OnPostCreateSession += OnGameSessionCreate;
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnMinion_ServerRPC(ulong clientID, int minionDataIndex, int targetHandDeckIndex)
    {
        //유저 입장에서 세션에 커넥트되는 시점에는 이미 세션이 돌아가고있는것과 마찬가지
        isSessionRunning = true;

#if UNITY_EDITOR
        Debug.Log("spawn data in deck of index " + minionDataIndex + " , hand index is " + targetHandDeckIndex);
#endif

        var spawned = SpawnMinion(clientID, minionDataIndex);
        if (spawned)
        {
            var playerData = playerDataByClientID[clientID];
            var randomMinionIndex = Random.Range(0, playerData.MinionDeck.Count);
            var RPCParam = new ClientRpcParams();
            RPCParam.Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            };
            handDeck.UpdateHandDeck_ClientRPC(targetHandDeckIndex, randomMinionIndex, RPCParam);
        }
    }

    private bool SpawnMinion(ulong clientID, int minionDataIndex, bool forcePurchaseEvenNoGem = false)
    {
        var playerData = PlayerDataByClientID[clientID];
        var minionData = playerData.MinionDeck[minionDataIndex];

        bool isPurchasable = forcePurchaseEvenNoGem || playerData.currentGem >= minionData.Stat[EStatName.GEM_COST].CurrentValue;
        if (isPurchasable)
        {
            var newMinion = Instantiate(MinionDataBaseIngame.Instance.MinionNetworkPrefabList[minionDataIndex]);
            playerData.AddMinionInstance(newMinion);
            var newMinionNetworkobject = newMinion.GetComponent<NetworkObject>();
            newMinionNetworkobject.SpawnWithOwnership(clientID);
            newMinion.gameObject.SetActive(true);
            newMinion.GetComponent<MinionInstance>().AssignOriginStat(minionData.Stat, newMinion);
            newMinion.GetComponent<AttackBehaviourBase>().SetOwner(newMinion.GetComponent<Minion>());

            // spawned minion's OnNetworkSpawn logic will automatically add itself to player data's minion instance list in client

            playerData.currentGem -= minionData.Stat[EStatName.GEM_COST].CurrentValue;
        }

        return isPurchasable;
    }


    //========================================================================================
    // Called from server
    //========================================================================================
    [ClientRpc]
    public void BroadCastNewPlayerConnection_ClientRPC(ulong clientID, int[] minionAssetIndexArr, int[] battalAbilityAssetIndexPerMinion, bool isBot)
    {
        //자기자신은 해당되지않음
        if (playerDataByClientID.ContainsKey(clientID)) return;

        var testDeck =
            MinionDataBaseIngame.Instance.GetMinionDeck(minionAssetIndexArr, battalAbilityAssetIndexPerMinion);
        var playerData = new PlayerData(testDeck, string.Empty, clientID, isBot);
        playerDataList.Add(playerData);
        playerDataByClientID.Add(clientID, playerData);
        onPostNewPlayerConnect?.Invoke(playerData);
    }
    
    //========================================================================================
    //========================================================================================

    [ClientRpc]
    public void BroadcastBeginGame_ClientRPC()
    {
        isGameRunning = true;
        onGameStart?.Invoke();
    }
    //========================================================================================

    //========================================================================================
    // Called clent to server
    //========================================================================================
    [ServerRpc(RequireOwnership = false)]
    public void ResponseConnect_ServerRPC(string playerSessionID, ulong clientID)
    {
        Debug.Log("ResponseConnect_ServerRPC");
        if (!IsServer) return;

        if (PlayerDataByClientID.ContainsKey(clientID)) return;
        Debug.Log(string.Format("server accept connection from ID {0}", clientID));
        //TODO : connect to server then server Generating PlayerData with get some data from DB with playerSessionID

        //첫 유저 입장시 행동
        if (playerDataList.Count == 0)
        {
            SessionStartTime.Value = Time.time;
            SessionTime.Value = 0f;
        }

        var tesstMinionIndexList = new List<int>();
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(1);
        tesstMinionIndexList.Add(2);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(5);

        var testMinionStatIndexList = new List<int>(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);


        var testDeck =
            MinionDataBaseIngame.Instance.GetMinionDeck(tesstMinionIndexList, testMinionStatIndexList);
        var newPlayerData = new PlayerData(testDeck, playerSessionID, clientID, false);
        newPlayerData.currentGem = 50;

        playerDataList.Add(newPlayerData);
        playerDataByClientID.Add(clientID, newPlayerData);
        BroadCastNewPlayerConnection_ClientRPC(clientID, tesstMinionIndexList.ToArray(), testMinionStatIndexList.ToArray(), false);

        var handDeckIndices = new int[4];
        for (int i = 0; i < handDeckIndices.Length; i++)
            handDeckIndices[i] = Random.Range(0, newPlayerData.MinionDeck.Count);

        var RPCParam = new ClientRpcParams();
        RPCParam.Send = new ClientRpcSendParams()
        {
            TargetClientIds = new ulong[] { clientID }
        };
        handDeck.SetHandDeck_ClientRPC(handDeckIndices, RPCParam);

        var playerObject = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);

        onPostNewPlayerConnect?.Invoke(newPlayerData);
    }

    private void OnGameSessionCreate()
    {
        isSessionRunning = true;
        SessionTime.Value = 0f;
        SessionStartTime.Value = Time.time;

        SessionUpdate().Forget();
    }

    private async UniTask SessionUpdate()
    {
        while (true)
        {
            SessionTime.Value = Time.time - SessionStartTime.Value;

            if (!isGameRunning)
            {
                bool canStartGame = playerDataList.Count == MaxUserCount || SessionTime.Value >= maxPreparationTime;

                // 게임 세션을 시작할수 있는지 체크 후 게임 세션 시작\
                if (canStartGame)
                {
                    if (playerDataList.Count < MaxUserCount)
                    {
                        for (int i = 0; i < MaxUserCount - playerDataList.Count; i++)
                        {
                            AddBot();
                        }
                    }

                    onGameStart?.Invoke();
                    GameUpdate().Forget();
                }
            }
            await UniTask.NextFrame();
        }
    }
    private void AddBot()
    {
        ulong localBotFakeClientID = 10000;
        string localBotPlayerSessionID = "IAMABEST";

        while (this.playerDataByClientID.ContainsKey(localBotFakeClientID)) localBotFakeClientID++;

        var tesstMinionIndexList = new List<int>();
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(1);
        tesstMinionIndexList.Add(2);
        tesstMinionIndexList.Add(0);
        tesstMinionIndexList.Add(5);

        var testMinionStatIndexList = new List<int>(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);
        testMinionStatIndexList.Add(0);


        var testDeck =
            MinionDataBaseIngame.Instance.GetMinionDeck(tesstMinionIndexList, testMinionStatIndexList);
        var newPlayerData = new PlayerData(testDeck, localBotPlayerSessionID, localBotFakeClientID, true);
        newPlayerData.currentGem = 50;

        playerDataList.Add(newPlayerData);
        playerDataByClientID.Add(localBotFakeClientID, newPlayerData);
        BroadCastNewPlayerConnection_ClientRPC(localBotFakeClientID, tesstMinionIndexList.ToArray(), testMinionStatIndexList.ToArray(), true);

        var handDeckIndices = new int[4];
        for (int i = 0; i < handDeckIndices.Length; i++)
            handDeckIndices[i] = Random.Range(0, newPlayerData.MinionDeck.Count);

        var RPCParam = new ClientRpcParams();
        RPCParam.Send = new ClientRpcSendParams()
        {
            TargetClientIds = new ulong[] { localBotFakeClientID }
        };
        handDeck.SetHandDeck_ClientRPC(handDeckIndices, RPCParam);

        var playerObject = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(localBotFakeClientID);

        onPostNewPlayerConnect?.Invoke(newPlayerData);
    }


    private async UniTask GameUpdate()
    {
        while (true)

        {
            GameTime.Value = Time.time - GameStartTime.Value;
            await UniTask.NextFrame();
        }
    }
    //========================================================================================

}



