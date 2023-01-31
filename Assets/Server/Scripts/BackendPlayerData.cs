using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public struct PlayerData
{
    public int Stage;
}

public static class BackendPlayerData
{
    public static PlayerData playerData;

    private static string tableName => "Player";

    public static bool IsDataEmpty() => Backend.GameData.GetMyData(tableName, new Where()).GetReturnValuetoJSON()["rows"].Count <= 0;

    public static void CreateOrLoadData()
    {
        var BRO = Backend.GameData.GetMyData(tableName, new Where());

        if (IsDataEmpty())
        {
            var param = new Param();
            param.Add("Stage", 1);
            Backend.GameData.Insert(tableName, param);
            playerData.Stage = 1;
        }
        else
        {
            for (int i = 0; i < BRO.Rows().Count; ++i)
            {
                var stage = BRO.Rows()[i]["Stage"]["N"].ToString();
                playerData.Stage = int.Parse(stage);
            }
        }
    }

    public static void UpdateData(PlayerData updateData)
    {
        var param = new Param();
        param.Add("Stage", updateData.Stage);
        Backend.GameData.Update(tableName, new Where(), param);
    }

    public static void UpdateData() => UpdateData(playerData);
}
