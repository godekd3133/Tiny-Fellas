using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerBootstrapper : MonoWeakSingleton<ServerBootstrapper>
{
    [SerializeField] private string gameSessionScenePath;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene(gameSessionScenePath, LoadSceneMode.Single);
        };
    }
}
