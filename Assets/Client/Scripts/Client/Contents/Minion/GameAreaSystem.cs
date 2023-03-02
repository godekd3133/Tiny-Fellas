using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 자기장 시스템에대한 구현입니다.
/// </summary>
public class GameAreaSystem : NetworkBehaviour
{
    [Header("Callback Functions")]
    [SerializeField] private UnityEvent onPhaseStart;
    [SerializeField] private UnityAction<int> onPhaseChange;

    [Header("Editable Initalize Values")]
    [SerializeField] private float startDelay;
    [SerializeField] private Vector3 startScale;
    [SerializeField] private GameObject areaGameObject;

    [SerializeField] private List<float> areaScalesPerPhase;
    [SerializeField] private List<float> areaReductionTimes;
    [SerializeField] private List<float> areaDurations;

    [Space(20), Header("Runtime Readonly")]
    [SerializeField] private int currentPhase;

    public float StartDelay { get { return startDelay; } }
    public Vector3 StartScale { get { return startScale; } }
    public List<float> AreaScalesPerPhase { get { return areaScalesPerPhase; } }
    public List<float> AreaReductionTimes { get { return areaReductionTimes; } }
    public List<float> AreaDurations { get { return areaDurations; } }

    public int CurrentPhase { get { return currentPhase; } }

    // TODO Session Start Callback
    private void OnSessionStart()
    {
        SessionArea().Forget();
    }

    private void Start()
    {
        OnSessionStart();
    }

    private async UniTask SessionArea()
    {
        areaGameObject.SetActive(false);
        currentPhase = -1;

        await UniTask.Delay(TimeSpan.FromSeconds(startDelay));
        // Enable Area Object
        onPhaseStart?.Invoke();
        areaGameObject.SetActive(true);
        areaGameObject.transform.localScale = startScale;

        for (int i = 0; i < areaScalesPerPhase.Count; i++)
        {
            currentPhase = i;
            onPhaseChange?.Invoke(currentPhase);
            await UniTask.Delay(TimeSpan.FromSeconds(areaDurations[i]));
            await Reduction(i);
        }
    }

    private async UniTask Reduction(int toPhase)
    {
        Tween scaleTween = areaGameObject.transform.DOScale(new Vector3(areaScalesPerPhase[toPhase],
                                      StartScale.y,
                                      areaScalesPerPhase[toPhase]), areaReductionTimes[toPhase]).SetEase(Ease.Linear);
        bool finished = false;
        scaleTween.onComplete += () => finished = true;
        scaleTween.Play();
        await UniTask.WaitUntil(() => finished);
    }
}

