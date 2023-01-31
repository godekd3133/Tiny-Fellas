using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider LoadingGauge;
    public AsyncOperation LoadingOp;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (LoadingOp != null && LoadingGauge != null)
        {
            LoadingGauge.value = LoadingOp.progress;
        }
    }
}
