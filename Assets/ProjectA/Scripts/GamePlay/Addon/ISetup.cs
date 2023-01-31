using UnityEngine;

public interface ISetup<T>
{
    void Setup(T table);
}

public static class SetUpExtension
{
    public static void AddTable(this UnitBase my, AttackInfo table)
    {
        if (!my.gameObject.GetComponent<AddonAttackable>())
            my.gameObject.AddComponent<AddonAttackable>().Init(table, my);
        else
            my.gameObject.GetComponent<AddonAttackable>().Setup(table);
    }

    public static void AddTable(this UnitBase my, MoveInfo table)
    {
        if (!my.gameObject.GetComponent<AddonMoveable>())
            my.gameObject.AddComponent<AddonMoveable>().Init(table, my);
        else
            my.gameObject.GetComponent<AddonMoveable>().Setup(table);
    }
}