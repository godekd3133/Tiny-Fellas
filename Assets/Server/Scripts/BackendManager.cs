using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using System.Linq;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }

    private void Init()
    {
        Backend.InitializeAsync(true, BRO =>
        {
            if (!BRO.IsSuccess())
            {
                print($"ErrorCode : {BRO.GetErrorCode()}, {BRO.GetMessage()}");
            }
        });
    }

    public void GetChart()
    {
        var BRO = Backend.Chart.GetChartContents("32267");
        if (BRO.IsSuccess())
        {
            for (int i = 0; i < BRO.Rows().Count; i++)
            {
                var user = BackendReturnObject.Flatten(BRO.Rows())[i];

                int stage = int.Parse(user["Stage"].ToString());
                double delay = double.Parse(user["Delay"].ToString());

                string strCode = user["Codes"].ToString();
                string[] arrCodes = strCode.Split(',');

                List<int> code = new List<int>();

            	for (int j = 0; j < arrCodes.Length; j++)
                	code.Add(int.Parse(arrCodes[j]));

                DataList.SetStageDelay(stage, delay);
                DataList.SetStageUnits(stage, code);
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        BackendPlayerData.UpdateData();
    }
    private void OnApplicationQuit()
    {
        BackendPlayerData.UpdateData();
    }
}
