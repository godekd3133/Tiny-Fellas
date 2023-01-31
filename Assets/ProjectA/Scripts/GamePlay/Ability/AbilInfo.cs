using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu, BoxGroup]
public class AbilInfo : ScriptableObject
{
    [SerializeField] private Sprite abilIcon;
    [SerializeField] private Color abilColor;

    [SerializeField] private string abilName;

    [SerializeField] private AbilBase abilPrefab;

    public Sprite Icon => abilIcon;

    public Color Color => abilColor;

    public string Name => abilName;

    public AbilBase Ability => abilPrefab;
}
