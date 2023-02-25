using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.Netcode;
using Unity.Netcode.Editor;
using UnityEngine;

/// <summary>
/// 자기장 시스템에대한 구현입니다.
/// </summary>
public class GameAreaSystem : NetworkBehaviour
{
    [Header("Editable Initalize Values")]
    [SerializeField] private float startDelay;
    [SerializeField] private List<float> areaScalesPerPhase;
    [SerializeField] private List<float> areaDurations;

    [Space(20), Header("Runtime Readonly")]
    [SerializeField] private float currentPhase;

    public float StartDelay { get { return startDelay; } }
    public List<float> AreaScalesPerPhase { get { return areaScalesPerPhase; } }
    public List<float> AreaDurations { get { return areaDurations; } }

    public float CurrentPhase { get { return currentPhase; } }

    private void Awake()
    {
    }

    private void Update()
    {

    }
}

