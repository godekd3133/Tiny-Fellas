using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class UnitList
{
    public static Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
    public static Tuple<int, string>[] units =
    {
        CreateUnitWithURL(1, "Assets/ProjectA/Table/Units/Test/Test1.asset"),
        CreateUnitWithURL(2, "Assets/ProjectA/Table/Units/Test/Test2.asset"),
        CreateUnitWithURL(3, "Assets/ProjectA/Table/Units/Test/Test3.asset"),
        CreateUnitWithURL(4, "Assets/ProjectA/Table/Units/Test/Test4.asset"),
        CreateUnitWithURL(5, "Assets/ProjectA/Table/Units/Test/Test5.asset"),
    };
    private static Tuple<int, string> CreateUnitWithURL(int code, string url)
    {
        return new Tuple<int, string>(code, url);
    }
}
