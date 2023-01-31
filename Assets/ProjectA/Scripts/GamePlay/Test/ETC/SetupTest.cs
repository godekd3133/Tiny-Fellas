using UnityEngine;
using Sirenix.OdinInspector;

// 이런식으로 사용하3
public class SetupTest : MonoBehaviour
{
    [Button("유닛 생성")]
    public void AddUnit(UnitBase prefab, Transform pos = null)
    {
        var o = Instantiate(prefab);
        if (pos)
            o.transform.position = pos.position;
    }

    [Button("유닛 정보 셋업")]
    public void SetUnit(UnitBase obj, UnitInfo table)
    {
        obj.Setup(table);
    }

    [Button("유닛 공격 추가 및 셋업")]
    public void SetAttack(UnitBase obj, AttackInfo table)
    {
        obj.AddTable(table);
    }

    [Button("유닛 이동 추가 및 셋업")]
    public void SetMove(UnitBase obj, MoveInfo table)
    {
        obj.AddTable(table);
    }
}