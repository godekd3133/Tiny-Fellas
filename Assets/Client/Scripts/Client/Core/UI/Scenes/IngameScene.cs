using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScene : UIScene
{
    [SerializeField] PlayerHandDeck playerHandDeck;

    public override void OnAdd()
    {
        base.OnAdd();
        GameSessionInstance.Instance.HandDeck = playerHandDeck;
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }



}
