using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerGemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    #if !UNITY_SERVER || UNITY_EDITOR
    private void Awake()
    {
        AssignGemChangeCallback();
    }


    private async UniTask AssignGemChangeCallback()
    {
        await UniTask.WaitUntil(waitTroopAdmimSpawn);
        NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<TroopAdmin>().AssignCallbackOnGemValueChange(
            (previous, current) =>
            {
                text.text = current.ToString();
            });
    }

    private bool waitTroopAdmimSpawn()
    {
        return NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject() != null;
    }
    #endif
}
