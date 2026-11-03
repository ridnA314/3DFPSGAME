using System.Collections;
using UnityEngine;

// 로그인 씬 인트로(검은 화면 페이드 → 패널 요소 순차 등장)와 로그인 ↔ 회원가입 패널 전환 연출.
public class UI_IntroSequence : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup _fadeOverlay;
    [SerializeField] private float _fadeDuration = 0.8f;

    [Header("Panels")]
    [SerializeField] private CanvasGroup _loginPanel;
    [SerializeField] private CanvasGroup _registerPanel;
    [SerializeField] private float _revealDelay = 1.4f;
    [SerializeField] private float _panelFadeDuration = 0.2f;

    [Header("Items")]
    [SerializeField] private float _itemDuration = 0.35f;
    [SerializeField] private float _itemStagger = 0.08f;
    [SerializeField] private float _itemSlideDistance = 40f;

    private PanelItems _loginItems;
    private PanelItems _registerItems;
    private Coroutine _transitionRoutine;

    private void Awake()
    {
        _loginItems = new PanelItems(_loginPanel.transform);
        _registerItems = new PanelItems(_registerPanel.transform);
    }

    // 다른 컴포넌트들의 Awake()가 끝난 뒤 초기 위치를 바꾸기 위해 Start()에서 시작한다.
    private void Start()
    {
        _registerPanel.gameObject.SetActive(false);
        _loginPanel.gameObject.SetActive(true);
        _loginItems.Hide(_itemSlideDistance);
        SetPanelInteractable(_loginPanel, false);

        _fadeOverlay.gameObject.SetActive(true);
        _fadeOverlay.alpha = 1f;
        _fadeOverlay.blocksRaycasts = true;

        _transitionRoutine = StartCoroutine(Intro_Coroutine());
    }

    public void ShowLoginPanel()
    {
        SwitchPanel(_registerPanel, _loginPanel, _loginItems);
    }

    public void ShowRegisterPanel()
    {
        SwitchPanel(_loginPanel, _registerPanel, _registerItems);
    }

    private void SwitchPanel(CanvasGroup from, CanvasGroup to, PanelItems toItems)
    {
        if (_transitionRoutine != null)
        {
            StopCoroutine(_transitionRoutine);
        }

        _transitionRoutine = StartCoroutine(SwitchPanel_Coroutine(from, to, toItems));
    }

    private IEnumerator Intro_Coroutine()
    {
        yield return Fade_Coroutine(_fadeOverlay, 1f, 0f, _fadeDuration);
        _fadeOverlay.blocksRaycasts = false;
        _fadeOverlay.gameObject.SetActive(false);

        yield return UIWait.Seconds_Coroutine(_revealDelay);
        yield return Reveal_Coroutine(_loginPanel, _loginItems);
        _transitionRoutine = null;
    }

    private IEnumerator SwitchPanel_Coroutine(CanvasGroup from, CanvasGroup to, PanelItems toItems)
    {
        SetPanelInteractable(from, false);
        yield return Fade_Coroutine(from, from.alpha, 0f, _panelFadeDuration);
        from.gameObject.SetActive(false);
        from.alpha = 1f;

        to.gameObject.SetActive(true);
        toItems.Hide(_itemSlideDistance);
        SetPanelInteractable(to, false);
        yield return Reveal_Coroutine(to, toItems);
        _transitionRoutine = null;
    }

    private IEnumerator Reveal_Coroutine(CanvasGroup panel, PanelItems items)
    {
        float totalDuration = _itemDuration + _itemStagger * Mathf.Max(0, items.Count - 1);
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            for (int i = 0; i < items.Count; i++)
            {
                float t = Mathf.Clamp01((elapsed - i * _itemStagger) / _itemDuration);
                items.SetProgress(i, UIEase.OutCubic(t), _itemSlideDistance);
            }

            yield return null;
        }

        items.Show();
        SetPanelInteractable(panel, true);
    }

    private IEnumerator Fade_Coroutine(CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        group.alpha = to;
    }

    private static void SetPanelInteractable(CanvasGroup panel, bool isInteractable)
    {
        panel.interactable = isInteractable;
        panel.blocksRaycasts = isInteractable;
    }

    // 패널의 직계 자식들을 순차 등장시키기 위한 위치/투명도 정보
    private class PanelItems
    {
        private readonly RectTransform[] _rects;
        private readonly CanvasGroup[] _groups;
        private readonly Vector2[] _basePositions;

        public int Count => _rects.Length;

        public PanelItems(Transform panel)
        {
            int count = panel.childCount;
            _rects = new RectTransform[count];
            _groups = new CanvasGroup[count];
            _basePositions = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                RectTransform rect = (RectTransform)panel.GetChild(i);
                _rects[i] = rect;
                _basePositions[i] = rect.anchoredPosition;
                _groups[i] = rect.TryGetComponent(out CanvasGroup group) ? group : rect.gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void Hide(float slideDistance)
        {
            for (int i = 0; i < Count; i++)
            {
                SetProgress(i, 0f, slideDistance);
            }
        }

        public void Show()
        {
            for (int i = 0; i < Count; i++)
            {
                SetProgress(i, 1f, 0f);
            }
        }

        public void SetProgress(int index, float progress, float slideDistance)
        {
            _groups[index].alpha = progress;
            _rects[index].anchoredPosition = _basePositions[index] + Vector2.down * (slideDistance * (1f - progress));
        }
    }
}
