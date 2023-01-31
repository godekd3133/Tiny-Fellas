using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataList
{
    public static Dictionary<int, ScriptableObject> allUnits = new Dictionary<int, ScriptableObject>();
    public static Dictionary<int, List<int>> stageUnits = new Dictionary<int, List<int>>();
    public static Dictionary<int, double> stageDelay = new Dictionary<int, double>();

    public static void SetUnits(int code , ScriptableObject unit)
    {
        Debug.Log("SetUnits " + code + " " + unit.name);
        allUnits.Add(code, unit);
    }
    public static ScriptableObject GetUnits(int code)
    {
        Debug.Log("GetUnits " + code + " " + allUnits[code].name);
        return allUnits[code];
    }
    public static void SetStageUnits(int stage, List<int> code)
    {
        stageUnits.Add(stage, code);
    }
    public static List<int> GetStageUnits(int stage)
    {
        return stageUnits[stage];
    }
    public static void SetStageDelay(int stage, double delay)
    {
        stageDelay.Add(stage, delay);
    }
    public static double GetStageDelay(int stage)
    {
        return stageDelay[stage];
    }
}
