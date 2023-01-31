using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMain : MonoBehaviour
{
    public static SystemMain Current { get; private set; }

    // 게임의 환경 설정 느낌s
    [System.Serializable]
    public class Settings
    {
        // To Do ...
    }

    // Address 에셋을 추가해서 여기서 전부 시작해주는 것도 고려 중...
}
