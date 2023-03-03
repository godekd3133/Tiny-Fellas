using System;

using Aws.GameLift;
using Aws.GameLift.Server;
using UnityEngine;
public class SDKInitializer : MonoWeakSingleton<SDKInitializer>
{
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField] private bool isLocalTest;

    private bool isConnected;
    private GameLift gameLift;

    private bool gameLiftRequestedTermination = false;

    public bool IsConnected
    {
        get => isConnected;
    }

    public bool IsLocalTest => isLocalTest;

    void Awake()
    {
        isConnected = false;
        Application.runInBackground = true;

        string sdkVersion = GameLiftServerAPI.GetSdkVersion().Result;
        Logger.SharedInstance.Write(":) SDK VERSION: " + sdkVersion);

        try
        {
            GenericOutcome initOutcome = GameLiftServerAPI.InitSDK();

            if (initOutcome.Success)
            {
                Logger.SharedInstance.Write(":) SERVER IS IN A GAMELIFT FLEET");
                ProcessReady();
            }
            else
            {
                isConnected = false;
                Logger.SharedInstance.Write(":( SERVER NOT IN A FLEET. GameLiftServerAPI.InitSDK() returned " + Environment.NewLine + initOutcome.Error.ErrorMessage);
            }
        }
        catch (Exception e)
        {
            Logger.SharedInstance.Write(":( SERVER NOT IN A FLEET. GameLiftServerAPI.InitSDK() exception " + Environment.NewLine + e.Message);
        }
    }
    private void ProcessReady()
    {
        try
        {
            ProcessParameters processParams = CreateProcessParameters();
            GenericOutcome processReadyOutcome = GameLiftServerAPI.ProcessReady(processParams);
            isConnected = processReadyOutcome.Success;

            if (processReadyOutcome.Success)
                Logger.SharedInstance.Write(":) PROCESSREADY SUCCESS.");
            else
                Logger.SharedInstance.Write(":( PROCESSREADY FAILED. ProcessReady() returned " + processReadyOutcome.Error.ToString());
        }
        catch (Exception e)
        {
            Logger.SharedInstance.Write(":( PROCESSREADY FAILED. ProcessReady() exception " + Environment.NewLine + e.Message);
        }
    }

    private ProcessParameters CreateProcessParameters()
    {
        var logParameters = new LogParameters();
        int port = EnviromentUtils.Port ?? 1;

        return new ProcessParameters(
             (gameSession) =>
            {
                Logger.SharedInstance.Write(":) GAMELIFT SESSION REQUESTED");
                // TODO: game session things
                AWSFleetManager.Instance.GenerateNewGameSession(gameSession);

                try
                {
                    GenericOutcome outcome = GameLiftServerAPI.ActivateGameSession();

                    if (outcome.Success)
                        Logger.SharedInstance.Write(":) GAME SESSION ACTIVATED");
                    else
                        Logger.SharedInstance.Write(":( GAME SESSION ACTIVATION FAILED. ActivateGameSession() returned " + outcome.Error.ToString());
                }
                catch (Exception e)
                {
                    Logger.SharedInstance.Write(":( GAME SESSION ACTIVATION FAILED. ActivateGameSession() exception " + Environment.NewLine + e.Message);
                }
            },
            (session) =>
            {

            }
           ,
             () =>
            {
                Logger.SharedInstance.Write(":| GAMELIFT PROCESS TERMINATION REQUESTED (OK BYE)");
                gameLiftRequestedTermination = true;
                Application.Quit();
            }
            ,

             () =>
             {
                 Logger.SharedInstance.Write(":) GAMELIFT HEALTH CHECK REQUESTED (HEALTHY)");
                 return true;
             },
            port, // tell the GameLift service which port to connect to this process on.
                  // unless we manage this there can only be one process per server.
            logParameters);
    }

    public void TerminateGameSession(bool processEnding)
    {
        if (gameLiftRequestedTermination)
        {
            // don't terminate game session if gamelift initiated process termination, just exit.
            Environment.Exit(0);
        }

        try
        {
            GenericOutcome outcome = GameLiftServerAPI.TerminateGameSession();

            if (outcome.Success)
            {
                Logger.SharedInstance.Write(":) GAME SESSION TERMINATED");

                if (processEnding)
                    ProcessEnding();
                else
                    ProcessReady();
            }
            else
            {
                Logger.SharedInstance.Write(":( GAME SESSION TERMINATION FAILED. TerminateGameSession() returned " +
                              outcome.Error.ToString());
            }
        }
        catch (Exception e)
        {
            Logger.SharedInstance.Write(":( GAME SESSION TERMINATION FAILED. TerminateGameSession() exception " +
                          Environment.NewLine + e.Message);
        }
    }

    private void ProcessEnding()
    {
        try
        {
            GenericOutcome outcome = GameLiftServerAPI.ProcessEnding();

            if (outcome.Success)
                Logger.SharedInstance.Write(":) PROCESSENDING");
            else
                Logger.SharedInstance.Write(":( PROCESSENDING FAILED. ProcessEnding() returned " + outcome.Error.ToString());
        }
        catch (Exception e)
        {
            Logger.SharedInstance.Write(":( PROCESSENDING FAILED. ProcessEnding() exception " + Environment.NewLine + e.Message);
        }
    }
#endif
}
