using System.IO;
using System.Net;
using Amazon.GameLift;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Aws.GameLift.Server.Model;

[RequireComponent( typeof(NetworkObject))]
public class AWSFleetManager : NetworkBehaviourSingleton<AWSFleetManager>
{
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField]
    private string gameSessionSettingPath = "Assets/Datas/Settinga/GameSessionSetting";

    private GameSession gameSession;
    private AmazonGameLiftClient gameLiftClient;
    private UnityTransport transport;
    private NetworkObject networkObject;
    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
#if UNITY_EDITOR
        if(SDKInitializer.Instance.IsLocalTest)
        NetworkManagerInstance.Instance.StartHost();
        Logger.SharedInstance.Write("Server starts as host");
#endif
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
        
        
        NetworkManagerInstance.Instance.StartServer();
        Logger.SharedInstance.Write("Server starts");
    }
#endif
    #if !UNITY_SERVER || UNITY_EDITOR

    [SerializeField] private FleetConnectionSetting_Client fleetConnectionSetting;
    public async UniTask<GameSession> ConnectToGameSession()
    {
        // TODO: AWS API Gateway
        string url = "";
        WebRequest request = HttpWebRequest.Create(url);  
        WebResponse response = request.GetResponse();  
        StreamReader reader = new StreamReader(response.GetResponseStream());  
        string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  
        //TODO : call lambda
       
        return null;
    }

    public void ConnectToGameSession_Teswt()
    {
        NetworkManagerInstance.Instance.StartClient();
    }

   #endif
}
