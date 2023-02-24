using System;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using Amazon.Runtime;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using GameSession = Amazon.GameLift.Model.GameSession;

[RequireComponent(typeof(UnityTransport), typeof(NetworkManager))]
public class AWSFleetManager : MonoWeakSingleton<AWSFleetManager>
{
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField]
    private string gameSessionSettingPath = "Assets/Datas/Settinga/GameSessionSetting";
    
    private GameSession gameSession;
    private UnityTransport transport;
    private NetworkManager networkManager;
    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnClientConnectedCallback += OnClientConnection;

    }
    
    private void OnClientConnection(ulong clientID)
    {
    }

    public  void GenerateNewGameSession(GameSession gameSession)
    {
        var gameSessionSetting = Resources.Load<GameSessionSetting>(gameSessionSettingPath);
        this.gameSession = gameSession;
        
        transport.ConnectionData.Address = gameSession.IpAddress;
        transport.ConnectionData.Port = System.Convert.ToUInt16(gameSession.Port);
        NetworkManager.Singleton.StartServer();
    }

    //#elif !UNITY_SERVER || UNITY_EDITOR

    [SerializeField] private FleetConnectionSetting_Client fleetConnectionSetting;
    public async UniTask<GameSession> ConnectToGameSession_Client()
    {
        
        var client = new AmazonGameLiftClient(new BasicAWSCredentials("ACCESS_KEY_ID", "SECRET_ACCESS_KEY"), Amazon.RegionEndpoint.APNortheast2);

        var requestSearchingGameSessionRequest = new SearchGameSessionsRequest();
        requestSearchingGameSessionRequest.FleetId = fleetConnectionSetting.FleetID;
        
        var response = await client.SearchGameSessionsAsync(requestSearchingGameSessionRequest);
        
        //TODO: add exception or handling logic for when no session found or add matchmaking logic
        var sessionList = response.GameSessions;

        // https://docs.aws.amazon.com/gamelift/latest/apireference/API_SearchGameSessions.html
        //TODO : find best session logic
        var session = sessionList[0];
        
        //TODO : Set PlayerID to GoogleID or something unique
        var playerSessionRequest = new CreatePlayerSessionRequest();
        playerSessionRequest.GameSessionId = session.GameSessionId;
        playerSessionRequest.PlayerData = "";
        playerSessionRequest.PlayerId = string.Empty;
        var playerSessionResponse = await client.CreatePlayerSessionAsync(playerSessionRequest);
        
            
        //TODO get adress from getting game instance from Fleet
        transport.ConnectionData.Address = session.IpAddress;
        transport.ConnectionData.Port = Convert.ToUInt16(session.Port);
        NetworkManager.Singleton.StartClient();

        return session;
    }


#endif
}
