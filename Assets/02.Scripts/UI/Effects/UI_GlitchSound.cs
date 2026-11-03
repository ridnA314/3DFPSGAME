using UnityEngine;

// 타이틀 글리치(UI_GlitchText.BurstStarted)에 맞춰 디지털 잡음을 낸다.
public class UI_GlitchSound : MonoBehaviour
{
    [SerializeField] private UI_GlitchText _glitchText;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [SerializeField, Range(0f, 1f)] private float _volume = 0.45f;
    [SerializeField] private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);

    private void OnEnable()
    {
        _glitchText.BurstStarted += OnBurstStarted;
    }

    private void OnDisable()
    {
        _glitchText.BurstStarted -= OnBurstStarted;
    }

    private void OnBurstStarted(bool isIntro)
    {
        if (_clip == null) return;

        _source.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
        _source.PlayOneShot(_clip, _volume);
    }
}
