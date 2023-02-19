using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using Amazon.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AWSFleetManager : MonoWeakSingleton<AWSFleetManager>
{
    [SerializeField]
    private string gameSessionSettingPath = "Assets/Datas/Settinga/GameSessionSetting";
    
    // #if UNITY_SERVER
    public async UniTask<GameSessionInstance> GenerateNewGameSession()
    {
        var gameSessionSetting = Resources.Load<GameSessionSetting>(gameSessionSettingPath);
        await SceneLoader.LoadScene(gameSessionSetting.ScenePath);
        return null;
    }
    // #endif
#if !UNITY_SERVER
    private string serverIP;
    private int serverPort;

    public string ServerIP => new(serverIP);

    public int ServerPort => serverPort;

    public AWSFleetManager()
    {
        var client = new AmazonGameLiftClient(new BasicAWSCredentials("ACCESS_KEY_ID", "SECRET_ACCESS_KEY"), Amazon.RegionEndpoint.APNortheast2);
        var describeInstancesRequest = new DescribeInstancesRequest
        {
            FleetId = "YOUR_FLEET_ID",
        };

        
    }
    #endif
}
