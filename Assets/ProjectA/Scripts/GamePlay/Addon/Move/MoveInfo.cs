using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class MoveInfo : ScriptableObject
{
    [SerializeField, Required] private MoveBase _movePrefab;

    public MoveBase MovePrefab => _movePrefab;
}