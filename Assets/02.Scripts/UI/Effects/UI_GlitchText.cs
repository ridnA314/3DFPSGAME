using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 신호 끊김 글리치. 평소에는 가만히 있다가 가끔 짧게 가로 띠로 잘려 어긋나고 빨강/청록 잔상이 생긴다.
// 인트로에서는 같은 글리치를 거치며 신호가 잡히듯 나타난다.
// 연출용 복사본(잔상 2개 + 띠 N개)은 실행 시 자식으로 생성되며 글리치 중에만 켜진다.
[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_GlitchText : MonoBehaviour
{
    [Header("Intro")]
    [SerializeField] private float _startDelay = 0.6f;
    [SerializeField] private float _introDuration = 0.45f;
    [SerializeField, Range(0f, 1f)] private float _introFlicker = 0.3f;

    [Header("Burst")]
    [SerializeField] private bool _loop = true;
    [SerializeField] private float _intervalMin = 8f;
    [SerializeField] private float _intervalMax = 12f;
    [SerializeField] private float _burstDuration = 0.2f;
    [SerializeField] private float _frameInterval = 0.04f;

    [Header("Slices")]
    [SerializeField] private int _sliceCount = 5;
    [SerializeField] private float _sliceOffset = 30f;
    [SerializeField, Range(0f, 1f)] private float _sliceShiftRatio = 0.6f;
    [SerializeField] private float _slicePadding = 120f;

    [Header("Color Split")]
    [SerializeField] private float _splitOffset = 8f;
    [SerializeField] private float _jitter = 2f;
    [SerializeField] private Color _ghostColorA = new Color(1f, 0.16f, 0.24f, 0.75f);
    [SerializeField] private Color _ghostColorB = new Color(0.16f, 0.9f, 1f, 0.75f);

    private TextMeshProUGUI _text;
    private CanvasGroup _glitchLayer;
    private TextMeshProUGUI _ghostA;
    private TextMeshProUGUI _ghostB;
    private TextMeshProUGUI[] _slices;
    private Coroutine _playRoutine;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        BuildGlitchLayer();
    }

    private void OnEnable()
    {
        _playRoutine = StartCoroutine(Play_Coroutine());
    }

    private void OnDisable()
    {
        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            _playRoutine = null;
        }

        EndBurst();
    }

    private IEnumerator Play_Coroutine()
    {
        _text.canvasRenderer.SetAlpha(0f);

        yield return new WaitForSecondsRealtime(_startDelay);
        yield return Burst_Coroutine(_introDuration, true);

        while (_loop)
        {
            yield return new WaitForSecondsRealtime(Random.Range(_intervalMin, _intervalMax));
            yield return Burst_Coroutine(_burstDuration, false);
        }
    }

    // fadeIn이면 글리치를 거치며 점점 또렷해진다(인트로).
    private IEnumerator Burst_Coroutine(float duration, bool fadeIn)
    {
        SyncCopies();
        _text.canvasRenderer.SetAlpha(0f);
        _glitchLayer.gameObject.SetActive(true);

        float elapsed = 0f;
        float nextShuffle = 0f;
        while (elapsed < duration)
        {
            if (elapsed >= nextShuffle)
            {
                Shuffle();
                nextShuffle += _frameInterval;
            }

            // 인트로에서는 점점 밝아지되 불규칙하게 깜빡인다.
            _glitchLayer.alpha = fadeIn ? Mathf.Clamp01(elapsed / duration + Random.Range(-_introFlicker, _introFlicker)) : 1f;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        EndBurst();
    }

    private void EndBurst()
    {
        if (_glitchLayer != null)
        {
            _glitchLayer.gameObject.SetActive(false);
        }

        _text.canvasRenderer.SetAlpha(1f);
    }

    private void Shuffle()
    {
        Vector2 jitter = Random.insideUnitCircle * _jitter;
        _ghostA.rectTransform.anchoredPosition = new Vector2(-_splitOffset, 0f) + jitter;
        _ghostB.rectTransform.anchoredPosition = new Vector2(_splitOffset, 0f) - jitter;

        foreach (TextMeshProUGUI slice in _slices)
        {
            float offset = Random.value < _sliceShiftRatio ? Random.Range(-_sliceOffset, _sliceOffset) : 0f;
            slice.rectTransform.anchoredPosition = new Vector2(offset, 0f);
        }
    }

    private void BuildGlitchLayer()
    {
        RectTransform layer = CreateRect("GlitchLayer", transform);
        Stretch(layer);
        _glitchLayer = layer.gameObject.AddComponent<CanvasGroup>();
        _glitchLayer.blocksRaycasts = false;
        _glitchLayer.interactable = false;

        // 잔상이 먼저 그려져 띠 뒤에 깔린다.
        _ghostA = CreateCopy("GhostA", layer, _ghostColorA);
        _ghostB = CreateCopy("GhostB", layer, _ghostColorB);
        Stretch(_ghostA.rectTransform);
        Stretch(_ghostB.rectTransform);

        // 각 띠는 가로로 넓게 잡은 RectMask2D이고, 안의 복사본은 원본과 같은 위치에 놓인다.
        _slices = new TextMeshProUGUI[_sliceCount];
        for (int i = 0; i < _sliceCount; i++)
        {
            RectTransform band = CreateRect("Slice" + i, layer);
            band.anchorMin = new Vector2(0f, (float)i / _sliceCount);
            band.anchorMax = new Vector2(1f, (float)(i + 1) / _sliceCount);
            band.offsetMin = new Vector2(-_slicePadding, 0f);
            band.offsetMax = new Vector2(_slicePadding, 0f);
            band.gameObject.AddComponent<RectMask2D>();

            TextMeshProUGUI copy = CreateCopy("Text", band, _text.color);
            RectTransform copyRect = copy.rectTransform;
            copyRect.anchorMin = new Vector2(0f, -i);
            copyRect.anchorMax = new Vector2(1f, _sliceCount - i);
            copyRect.offsetMin = new Vector2(_slicePadding, 0f);
            copyRect.offsetMax = new Vector2(-_slicePadding, 0f);
            _slices[i] = copy;
        }

        layer.gameObject.SetActive(false);
    }

    private TextMeshProUGUI CreateCopy(string name, Transform parent, Color color)
    {
        RectTransform rect = CreateRect(name, parent);
        TextMeshProUGUI copy = rect.gameObject.AddComponent<TextMeshProUGUI>();
        copy.font = _text.font;
        copy.fontSharedMaterial = _text.fontSharedMaterial;
        copy.fontSize = _text.fontSize;
        copy.fontStyle = _text.fontStyle;
        copy.alignment = _text.alignment;
        copy.characterSpacing = _text.characterSpacing;
        copy.wordSpacing = _text.wordSpacing;
        copy.textWrappingMode = _text.textWrappingMode;
        copy.overflowMode = _text.overflowMode;
        copy.margin = _text.margin;
        copy.color = color;
        copy.raycastTarget = false;
        copy.text = _text.text;
        return copy;
    }

    private void SyncCopies()
    {
        _ghostA.text = _text.text;
        _ghostB.text = _text.text;
        foreach (TextMeshProUGUI slice in _slices)
        {
            slice.text = _text.text;
        }
    }

    private static RectTransform CreateRect(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.layer = parent.gameObject.layer;
        RectTransform rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        return rect;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
