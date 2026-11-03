using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 버튼 호버 시 확대 + 테두리, 누르면 눌렸다가 복귀하는 반응.
public class UI_ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Selectable _selectable;
    [SerializeField] private Outline _outline;
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _pressScale = 0.95f;
    [SerializeField] private float _scaleSpeed = 18f;

    private bool _isHovered;
    private Coroutine _scaleRoutine;

    private void OnDisable()
    {
        _isHovered = false;
        _scaleRoutine = null;
        transform.localScale = Vector3.one;
        SetOutline(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        _isHovered = true;
        SetOutline(true);
        ScaleTo(_hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        SetOutline(false);
        ScaleTo(1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        ScaleTo(_pressScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ScaleTo(_isHovered ? _hoverScale : 1f);
    }

    private bool IsInteractable()
    {
        return _selectable == null || _selectable.IsInteractable();
    }

    private void SetOutline(bool isActive)
    {
        if (_outline != null)
        {
            _outline.enabled = isActive;
        }
    }

    private void ScaleTo(float targetScale)
    {
        if (!isActiveAndEnabled) return;

        if (_scaleRoutine != null)
        {
            StopCoroutine(_scaleRoutine);
        }

        _scaleRoutine = StartCoroutine(Scale_Coroutine(targetScale));
    }

    private IEnumerator Scale_Coroutine(float targetScale)
    {
        Vector3 target = new Vector3(targetScale, targetScale, 1f);
        while ((transform.localScale - target).sqrMagnitude > 0.00001f)
        {
            float t = 1f - Mathf.Exp(-_scaleSpeed * Time.unscaledDeltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, target, t);
            yield return null;
        }

        transform.localScale = target;
        _scaleRoutine = null;
    }
}
