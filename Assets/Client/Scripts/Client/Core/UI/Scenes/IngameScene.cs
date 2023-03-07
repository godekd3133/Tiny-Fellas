using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScene : UIScene
{
    [SerializeField] Button connectionButton;

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
        AWSFleetManager.Instance.ConnectToGameSession_Test();
    }
}
