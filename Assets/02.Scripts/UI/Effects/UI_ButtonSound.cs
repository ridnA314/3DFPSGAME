using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 버튼/입력창에 마우스를 올리거나 클릭할 때 효과음을 낸다. AudioSource는 씬의 UI 전용 소스를 공유한다.
public class UI_ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private Selectable _selectable;
    [SerializeField] private AudioClip _hoverClip;
    [SerializeField] private AudioClip _clickClip;
    [SerializeField, Range(0f, 1f)] private float _hoverVolume = 0.35f;
    [SerializeField, Range(0f, 1f)] private float _clickVolume = 0.8f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Play(_hoverClip, _hoverVolume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Play(_clickClip, _clickVolume);
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip == null || _source == null) return;
        if (_selectable != null && !_selectable.IsInteractable()) return;

        _source.PlayOneShot(clip, volume);
    }
}
