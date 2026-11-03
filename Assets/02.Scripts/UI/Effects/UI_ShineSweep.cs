using System.Collections;
using UnityEngine;

// 버튼 위로 빛줄기가 주기적으로 지나가는 연출. 부모(버튼)에 RectMask2D가 있어야 빛이 버튼 밖으로 나가지 않는다.
public class UI_ShineSweep : MonoBehaviour
{
    [SerializeField] private RectTransform _shine;
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _interval = 3.5f;
    [SerializeField] private float _sweepDuration = 0.6f;

    private RectTransform _area;
    private Coroutine _sweepRoutine;

    private void Awake()
    {
        _area = (RectTransform)transform;
    }

    private void OnEnable()
    {
        _shine.gameObject.SetActive(false);
        _sweepRoutine = StartCoroutine(Sweep_Coroutine());
    }

    private void OnDisable()
    {
        if (_sweepRoutine != null)
        {
            StopCoroutine(_sweepRoutine);
            _sweepRoutine = null;
        }

        _shine.gameObject.SetActive(false);
    }

    private IEnumerator Sweep_Coroutine()
    {
        yield return new WaitForSecondsRealtime(_startDelay);

        while (true)
        {
            float travel = (_area.rect.width + _shine.rect.width) * 0.5f;
            Vector2 position = _shine.anchoredPosition;
            _shine.gameObject.SetActive(true);

            float elapsed = 0f;
            while (elapsed < _sweepDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / _sweepDuration);
                position.x = Mathf.Lerp(-travel, travel, t);
                _shine.anchoredPosition = position;
                yield return null;
            }

            _shine.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(_interval);
        }
    }
}
