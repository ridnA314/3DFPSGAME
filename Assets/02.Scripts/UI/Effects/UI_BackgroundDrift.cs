using UnityEngine;

// 배경 이미지를 천천히 확대/이동시키는 켄 번스 효과. 확대된 만큼만 이동해 가장자리가 드러나지 않는다.
public class UI_BackgroundDrift : MonoBehaviour
{
    [SerializeField] private float _maxScale = 1.08f;
    [SerializeField] private float _period = 20f;
    [SerializeField] private Vector2 _driftAmount = new Vector2(40f, 20f);
    [SerializeField] private float _verticalFrequencyRatio = 0.7f;

    private RectTransform _rectTransform;
    private Vector2 _basePosition;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _basePosition = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float phase = Time.unscaledTime * Mathf.PI * 2f / _period;
        float zoom = (1f - Mathf.Cos(phase)) * 0.5f;

        float scale = Mathf.Lerp(1f, _maxScale, zoom);
        _rectTransform.localScale = new Vector3(scale, scale, 1f);

        Vector2 drift = new Vector2(Mathf.Sin(phase) * _driftAmount.x, Mathf.Sin(phase * _verticalFrequencyRatio) * _driftAmount.y);
        _rectTransform.anchoredPosition = _basePosition + drift * zoom;
    }
}
