using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    private SpawnerData data = new SpawnerData();
    private Coroutine spawnCoroutine = null;
    private void Start()
    {
        UpdateSpawner();
    }

    public void UpdateSpawner()
    {
        data.UpdateSpawnerData();
        if(!StopSpawner())
        {
            spawnCoroutine = StartCoroutine(SpawnWithDelay(data.delay));
        }
    }

    public bool StopSpawner()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
            return true;
        }
        return false;
    }

    IEnumerator SpawnWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            int maxCount = data.units.Count;
            int spawnUnitCount = Random.Range(1, maxCount);
            ScriptableObject spawnUnitScritable = data.units[spawnUnitCount];
            ObjectPooler.SpawnFromPool<UnitBase>("Enemy", transform.position).Setup((UnitInfo)spawnUnitScritable);
        }
    }
}
