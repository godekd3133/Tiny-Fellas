using System;
using System.IO;
using System.Net;
using Amazon.GameLift;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Aws.GameLift.Server.Model;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class AWSFleetManager : MonoWeakSingleton<AWSFleetManager>
{
    [SerializeField] private string gameSessionScenePath = "Assets/Client/Scenes/IngameScene.unity";
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField]
    private string gameSessionSettingPath = "Assets/Datas/Settinga/GameSessionSetting";

    private GameSession gameSession;
    private AmazonGameLiftClient gameLiftClient;
    private NetworkObject networkObject;
    private void Start()
    {
        DontDestroyOnLoad(transform.parent);
        if (SDKInitializer.Instance.IsLocalTest)
        {
#if UNITY_EDITOR
            // NetworkManagerInstance.Instance.StartHost();
            // Logger.SharedInstance.Write(string.Format("Server starts as host client id is {0}", OwnerClientId));
#endif
#if UNITY_SERVER
            NetworkManager.Singleton.OnServerStarted += () =>
            {
                NetworkManager.Singleton.SceneManager.LoadScene(gameSessionScenePath, LoadSceneMode.Single);
            };
            
        NetworkManagerInstance.Instance.ConnectionApprovalCallback += (request, response) =>
        {
            bool alreadyConnected =
                GameSessionInstance.Instance.PlayerDataByClientID.ContainsKey(request.ClientNetworkId);
            response.Approved = !alreadyConnected;
            response.Pending = false;
            
        };
        NetworkManager.Singleton.StartServer();
        Logger.SharedInstance.Write(string.Format("Server starts as server"));
#endif
        }
    }
    
    public  void GenerateNewGameSession(GameSession gameSession)
    {
        var gameSessionSetting = Resources.Load<GameSessionSetting>(gameSessionSettingPath);
        this.gameSession = gameSession;
        var transporter = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transporter.ConnectionData.Address = gameSession.IpAddress;
        transporter.ConnectionData.Port = Convert.ToUInt16(gameSession.Port);
        
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene(gameSessionScenePath, LoadSceneMode.Single);
        };

       NetworkManager.Singleton.ConnectionApprovalCallback += (request, response) =>
        {
            bool alreadyConnected =
                GameSessionInstance.Instance.PlayerDataByClientID.ContainsKey(request.ClientNetworkId);
            response.Approved = !alreadyConnected;
            response.Pending = false;
        };
        
        
        NetworkManager.Singleton.StartServer();
       gameLiftClient = new AmazonGameLiftClient("AKIA3MTR52R2BGL7MOGB","ENfwYnCa4B20pg1ro+r1VJDetnOarvEA4DjhGzgv");
        
        Logger.SharedInstance.Write("Server Start");
    }
#endif
    #if !UNITY_SERVER || UNITY_EDITOR

    [SerializeField] private FleetConnectionSetting_Client fleetConnectionSetting;
    public async UniTask<GameSession> ConnectToGameSession()
    {
        // TODO: AWS API Gateway
        string url = "";
        WebRequest request = HttpWebRequest.Create(url);  
        WebResponse response = await request.GetResponseAsync();  
        StreamReader reader = new StreamReader(response.GetResponseStream());  
        string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  
        //TODO : call lambda
        OnConnectionResponse();
        return null;
    }

    public void ConnectToGameSession_Test()
    {
        Debug.Log(string.Format("trying to call Connection to server"));
        if(NetworkManagerInstance.Instance.IsConnectedClient) return;
        
        Debug.Log(string.Format("start client and call Connection funtion to server"));
        NetworkManagerInstance.Instance.StartClient();
        OnConnectionResponse();
    }

    private async UniTask OnConnectionResponse()
    {
        await UniTask.WaitUntil(() => NetworkManagerInstance.Instance.IsConnectedClient);
        GameSessionInstance.Instance.Connect_ServerRPC("125251245L", NetworkManagerInstance.Instance.LocalClientId);
        Debug.Log("connected to server!");
    }

   #endif
}
