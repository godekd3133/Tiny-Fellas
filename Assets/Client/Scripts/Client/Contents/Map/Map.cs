using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MeshRenderer renderer;

    public Vector3 GetMapSize() => new Vector3(renderer.bounds.size.x - 1.5f,
                                               renderer.bounds.size.y,
                                               renderer.bounds.size.z - 1.5f);

    public Vector3 GetRandomPosition()
    {
        Vector3 mapSize = GetMapSize();
        Vector3 randomPosition;
        randomPosition.x = ((float)UnityEngine.Random.Range(0, 100) / 100f - 0.5f) * mapSize.x;
        randomPosition.y = 0;
        randomPosition.z = ((float)UnityEngine.Random.Range(0, 100) / 100f - 0.5f) * mapSize.z;
        return randomPosition;
    }

}
