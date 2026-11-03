using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 팝업이 켜질 때(OnEnable) 배경 딤을 페이드하고 패널을 튀어나오듯 등장시킨다. timeScale = 0인 일시정지 중에도 동작한다.
public class UI_PopupAnimator : MonoBehaviour
{
    [SerializeField] private Graphic _dim;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private CanvasGroup _panelGroup;
    [SerializeField] private float _duration = 0.35f;
    [SerializeField] private float _startScale = 0.8f;

    private float _dimAlpha;

    private void Awake()
    {
        _dimAlpha = _dim.color.a;
    }

    private void OnEnable()
    {
        StartCoroutine(Open_Coroutine());
    }

    private void OnDisable()
    {
        SetProgress(1f, 1f);
    }

    private IEnumerator Open_Coroutine()
    {
        SetProgress(0f, 0f);

        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);
            SetProgress(t, UIEase.OutBack(t));
            yield return null;
        }

        SetProgress(1f, 1f);
    }

    private void SetProgress(float fade, float scaleProgress)
    {
        Color color = _dim.color;
        color.a = _dimAlpha * fade;
        _dim.color = color;

        _panelGroup.alpha = fade;
        float scale = Mathf.LerpUnclamped(_startScale, 1f, scaleProgress);
        _panel.localScale = new Vector3(scale, scale, 1f);
    }
}
