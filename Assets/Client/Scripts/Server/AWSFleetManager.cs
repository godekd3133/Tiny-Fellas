using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift;
using Aws.GameLift.Server.Model;
using Amazon.Runtime;
using Aws.GameLift.Server;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UnityTransport))]
public class AWSFleetManager : MonoWeakSingleton<AWSFleetManager>
{
    [SerializeField]
    private string gameSessionSettingPath = "Assets/Datas/Settinga/GameSessionSetting";
    
     #if UNITY_SERVER || UNITY_EDITOR
    private GameSession gameSession;
    private UnityTransport transport;
    private NetworkManager networkManager;
    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        transport = transport ?? GetComponent<UnityTransport>();
        transport.StartServer();
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
        transport.StartServer();
        NetworkManager.Singleton.StartServer();
    }
    #endif

    public AWSFleetManager()
    {
#if UNITY_SERVER || UNITY_EDITOR
        {
            
        };
        
        #endif
        
        #if !UNITY_SERVER || UNITY_EDITOR
        
         
        var unityNetworkClient = new NetworkClient();
        StartClient();
        var client = new AmazonGameLiftClient(new BasicAWSCredentials("ACCESS_KEY_ID", "SECRET_ACCESS_KEY"), Amazon.RegionEndpoint.APNortheast2);
        client.CreatePlayerSession(new CreatePlayerSessionRequest());
        var describeInstancesRequest = new DescribeInstancesRequest
        {
            FleetId = "YOUR_FLEET_ID",
        };
        
        #endif
    }
    
}
