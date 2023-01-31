using UnityEngine;

public class GameEntry : MonoBehaviour
{
    private void Start() => Init();
    private void Init()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 화면 비율
        {
            float screenHeight = Screen.height;
            float screenWidth = Screen.width;
            float ratio = screenWidth / screenHeight;
            int targetWidth = ratio > 1.9f ? 1080 : 1920;
            int targetHeight = (int)(targetWidth * ((float)Screen.height / Screen.width));
            Screen.SetResolution(targetWidth, targetHeight, true);
        }
    }
}
