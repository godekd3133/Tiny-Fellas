using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataFinder
{
    public static List<ScriptableObject> FindUnits(int stage)
    {
        List<ScriptableObject> units = new List<ScriptableObject>();
        foreach(var iter in DataList.GetStageUnits(stage))
        {
            units.Add(DataList.GetUnits(iter));
        }
        return units;
    }
}
