using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 입력창이 선택되면 테두리 글로우를 켜고, 입력 오류 시 Shake()로 좌우로 흔든다.
[RequireComponent(typeof(TMP_InputField))]
public class UI_InputFieldFeedback : MonoBehaviour
{
    [SerializeField] private Outline _glow;
    [SerializeField] private float _shakeDuration = 0.35f;
    [SerializeField] private float _shakeStrength = 12f;
    [SerializeField] private float _shakeFrequency = 40f;

    private TMP_InputField _inputField;
    private RectTransform _rectTransform;
    private Vector2 _basePosition;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _rectTransform = (RectTransform)transform;
        _basePosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        _inputField.onSelect.AddListener(OnSelect);
        _inputField.onDeselect.AddListener(OnDeselect);
        _glow.enabled = false;
    }

    private void OnDisable()
    {
        _inputField.onSelect.RemoveListener(OnSelect);
        _inputField.onDeselect.RemoveListener(OnDeselect);

        _shakeRoutine = null;
        _rectTransform.anchoredPosition = _basePosition;
        _glow.enabled = false;
    }

    public void Shake()
    {
        if (!isActiveAndEnabled) return;

        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }

        _shakeRoutine = StartCoroutine(Shake_Coroutine());
    }

    private void OnSelect(string text)
    {
        _glow.enabled = true;
    }

    private void OnDeselect(string text)
    {
        _glow.enabled = false;
    }

    private IEnumerator Shake_Coroutine()
    {
        float elapsed = 0f;
        while (elapsed < _shakeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float damping = 1f - elapsed / _shakeDuration;
            float offset = Mathf.Sin(elapsed * _shakeFrequency) * _shakeStrength * damping;
            _rectTransform.anchoredPosition = _basePosition + new Vector2(offset, 0f);
            yield return null;
        }

        _rectTransform.anchoredPosition = _basePosition;
        _shakeRoutine = null;
    }
}
