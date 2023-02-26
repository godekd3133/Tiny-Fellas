using System;
using System.Collections.Generic;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Components;
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
    private AmazonGameLiftClient gameLiftClient;
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
        
       gameLiftClient = new AmazonGameLiftClient("AKIA3MTR52R2BGL7MOGB","ENfwYnCa4B20pg1ro+r1VJDetnOarvEA4DjhGzgv");
        // 
        
        
        transport.ConnectionData.Address = gameSession.IpAddress;
        transport.ConnectionData.Port = System.Convert.ToUInt16(gameSession.Port);
        NetworkManager.Singleton.StartServer();
    }
#endif
    #if !UNITY_SERVER || UNITY_EDITOR

    [SerializeField] private FleetConnectionSetting_Client fleetConnectionSetting;
    public async UniTask<GameSession> ConnectToGameSession_Client()
    {
        //TODO : call lambda
       
        return null;
    }

   #endif
}
