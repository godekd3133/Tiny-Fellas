using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : UIScene
{
    [SerializeField] Button connectionButton;

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
        connectionButton.onClick.AddListener(OnConnectionButtonDown);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        connectionButton.onClick.RemoveListener(OnConnectionButtonDown);
    }

    private void OnConnectionButtonDown()
    {
        SetUpLobby();
    }

    private async Task SetUpLobby()
    {


        AWSFleetManager.Instance.ConnectToGameSession_Test();
        this.gameObject.SetActive(false);
    }


    // public void OnStartGame()
    // {
    //     SceneLoader.LoadScene("IngameScene");
    //     SceneLoader.current.AdditionalLoadingTasks.Add(OnLoadGame);
    // }

    // public async Task OnLoadGame()
    // {
    //     await ResourceManager.instance.PrepareLoading();

    //     TaskOperation operation = new TaskOperation();

    //     //TODO 여기에 로딩 큐를 추가하세요
    //     //EX) operation.AddRange(UnitList.units.Select(data => ResourceManager.instance.LoadScriptableObject(data.Item2)));
    //     //EX) operation.Add(ResourceManager.instance.LoadScriptableObject("Dsadsa"));   


    //     //////////////////////////////
    //     operation.Run();
    //     while (true)
    //     {
    //         if (operation.isRuntimeOperation == false) break;

    //         await UniTask.NextFrame();
    //     }
    // }
}
