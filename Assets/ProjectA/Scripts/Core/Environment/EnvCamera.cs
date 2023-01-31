using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnvCamera : MonoBehaviour
{
    public static EnvCamera Current { get; private set; }

    [System.Serializable]
    public class Settings
    {
        public Transform Target;
        public Vector3 Offset;

        public Settings(Transform target = null, Vector3? offset = null)
        {
            Target = target;
            Offset = offset ?? Vector3.zero;
        }
    }
    public Settings Follower = new Settings();

    [SerializeField] private AnimationCurve _shakeCurve;
    [SerializeField] private Transform _shakeBase;
    [SerializeField] private Camera _worldCamera;

    [SerializeField, Range(0, 60)] private float _moveDampPower;

    private void OnEnable() => Current = this;

    public void SetTarget(Settings settings) => Follower = settings;

    private void LateUpdate()
    {
        if (Follower != null)
            transform.position = Vector3.Lerp(transform.position, Follower.Target.position + Follower.Offset, _moveDampPower * Time.deltaTime);

        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = transform.forward;
    }

    public Vector3 GetRelative(Vector3 direction)
    {
        return transform.rotation * direction;
    }

    public Vector3 GetRelativeInverse(Vector3 direction)
    {
        return Quaternion.Inverse(transform.rotation) * direction;
    }

    private Coroutine shakeCoroutine;
    public void Shake(float power)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeRoutine(power));
    }

    public void CameraViewAdd(float add)
    {
        _worldCamera.fieldOfView += add;
    }

    private IEnumerator ShakeRoutine(float power)
    {
        float timer = 0;
        float maxTime = _shakeCurve[_shakeCurve.length - 1].time;
        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            float randomValue = _shakeCurve.Evaluate(timer) * power;
            _shakeBase.localPosition = Random.insideUnitSphere * Random.Range(-randomValue, randomValue);
            yield return null;
        }
    }
}
