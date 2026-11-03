using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectManager : SingletoneBehaviour<ScreenEffectManager>
{
    [SerializeField] private Image _hitFlash;
    [SerializeField] private float _hitEffectDuration;
    [SerializeField] private float _flashAlpha;

    private void Start()
    {
        SetAlpha(0);
        Player player = GameObject.FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.PlayEffect += ShowFlash;
        }
    }

    private void ShowFlash()
    {
        SetAlpha(_flashAlpha);

        //StartCoroutine(Flash_Coroutine());
        StartCoroutine(FlashFadeOut_Coroutine());
    }

    private void SetAlpha(float flashAlpha)
    {
        Color color = _hitFlash.color;
        color.a = flashAlpha;
        _hitFlash.color = color;
    }

    private IEnumerator FlashFadeOut_Coroutine()
    {
        // 1. show that alpha value is 0.4
        float timer = 0f;
        float alpha;

        // 2. implement disappear by changing alpha value to zero
        while (timer < _hitEffectDuration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(_flashAlpha, 0f, timer / _hitEffectDuration);
            SetAlpha(alpha);

            yield return null; // rest just one frame
        }
    }
}