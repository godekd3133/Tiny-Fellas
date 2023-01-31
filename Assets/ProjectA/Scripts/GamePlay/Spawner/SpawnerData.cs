using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnerData : MonoBehaviour
{
    public int stage;
    public float delay;
    public int unitKind;
    public List<ScriptableObject> units = new List<ScriptableObject>();

    public void UpdateSpawnerData()
    {
        stage = BackendPlayerData.playerData.Stage;
        Debug.Log("Juno1234 : 스테이지 " + stage);
        units = DataFinder.FindUnits(stage);
        delay = (float)DataList.GetStageDelay(stage);
        unitKind = units.Count;
        UnitInfo temp = (UnitInfo)units[0];
        Debug.Log("Juno1234 : 스테이지 " + stage + " 첫 유닛 이름 : " + temp.Name + " 딜레이 : " + (float)delay + " 유닛 종류 수 : " + unitKind);
    }
}
