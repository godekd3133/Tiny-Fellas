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
}
