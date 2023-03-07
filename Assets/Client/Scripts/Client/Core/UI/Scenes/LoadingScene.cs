using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScene : UIScene
{
    [SerializeField] TextMeshProUGUI loadingPercentageText;
    [SerializeField] Slider loadingGauge;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    public override void OnAdd()
    {
        base.OnAdd();
        StartLoadProcess();
    }

    

    private async Task SetUpLobby()
    {
        await RootCanvas.instance.SetScene("UIScene/LobbyScene.prefab");
    }

    public Task StartLoadProcess() => UpdateGameDataFlow().Then(DownLoadResourceFlow)
                                                          .Then(UseLoginFlow)
                                                          .Then(CompletedLoadFlow)
                                                          .Finally(SetUpLobby)
                                                          .Catch(e => Debug.LogException(e));

    private async Task UpdateGameDataFlow()
    {
        loadingGauge.value = 0f;
        loadingPercentageText.text = $"Update Resources : {0}%";
        await ResourceManager.instance.PrepareLoading();
        ;
        loadingGauge.value = 0.1f;
        loadingPercentageText.text = $"Update Resources : {10}%";

        TaskOperation operation = new TaskOperation();

        //TODO 여기에 로딩 큐를 추가하세요
        //EX) operation.AddRange(UnitList.units.Select(data => ResourceManager.instance.LoadScriptableObject(data.Item2)));
        //EX) operation.Add(ResourceManager.instance.LoadScriptableObject("Dsadsa"));
        operation.Add(ResourceManager.instance.LoadPrefab("UIScene/LobbyScene.prefab"));
        operation.Add(ResourceManager.instance.LoadPrefab("UIScene/InGameScene.prefab"));

        //  operation.AddRange(UnitList.units.Select(data => ResourceManager.instance.LoadScriptableObject(data.Item2).Then((res) => DataList.SetUnits(data.Item1, res))));

        //////////////////////////////
        operation.Run();
        while (true)
        {
            loadingPercentageText.text = $"Update Resources : {0.1f + operation.op * 0.9f*100f}%";

            if (operation.isRuntimeOperation == false) break;
            loadingGauge.value = 0.1f + operation.op * 0.9f;


            await UniTask.NextFrame();
        }
        await UniTask.Delay(1000);
    }
    private async Task DownLoadResourceFlow()
    {
        loadingGauge.value = 0f;
        loadingPercentageText.text = "Resource Download";

        await UniTask.Delay(500);
        loadingGauge.value = 1f;
        await UniTask.Delay(500);
    }
    private async Task UseLoginFlow()
    {
        loadingGauge.value = 0f;
        loadingPercentageText.text = "Try Login";
        await UniTask.Delay(500);
        loadingGauge.value = 1f;
        await UniTask.Delay(500);
    }

    private async Task CompletedLoadFlow()
    {
        loadingGauge.value = 0f;
        loadingPercentageText.text = "Finish Login";
        await UniTask.Delay(500);
        loadingGauge.value = 1f;
        await UniTask.Delay(500);
    }
}
