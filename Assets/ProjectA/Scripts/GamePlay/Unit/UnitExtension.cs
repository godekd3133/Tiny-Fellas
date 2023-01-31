using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using U = eUNIT_TYPE;

public enum eUNIT_TYPE
{
    Default, Player, Enemy, Friendly, Neutral,
    MAX
}

public static class UnitExtension
{
    public static U[] GetInverseTypes(this eUNIT_TYPE type)
    {
        switch (type)
        {
            case U.Default:
                return new[] { U.Player, U.Enemy, U.Neutral };

            case U.Player:
                return new[] { U.Default, U.Enemy, U.Neutral };

            case U.Enemy:
                return new[] { U.Default, U.Player, U.Neutral };

            case U.Friendly:
            case U.Neutral:
            case U.MAX:
            default:
                return null;
        }
    }

    public static bool IsFightWith(this eUNIT_TYPE my, U other) 
    {
        Debug.Log($"ifw : {my} : {other} : {my.GetInverseTypes().Contains(other)}");

        return my.GetInverseTypes().Contains(other); 
    }

    public static IEnumerable<UnitBase> GetUnits(this eUNIT_TYPE type)
    {
        return UnitBase.Units.Where(_ => _.My.Stat.Type == type);
    }

    public static UnitBase GetFindNearTarget(this eUNIT_TYPE type, GameObject me)
    {
        Debug.Log("gfnt");

        // 나중에 수정 예정
        return UnitBase.Units.Where(_ => _ && me.GetComponent<UnitBase>().My.Stat.Type.IsFightWith(_.My.Stat.Type))
              .OrderBy(_ => (me.transform.position - _.transform.position).sqrMagnitude)
              .FirstOrDefault();
    }
}
