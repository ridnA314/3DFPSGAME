using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectManager : MonoBehaviour
{
    private static ScreenEffectManager _instance;
    public static ScreenEffectManager Instance => _instance;

    [SerializeField] private Image _hitFlash;
    [SerializeField] private float _hitEffectDuration;
    [SerializeField] private float _flashAlpha;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Color color = _hitFlash.color;
        color.a = 0;
        _hitFlash.color = color;
    }

    public void ShowFlash()
    {
        Color color = _hitFlash.color;
        color.a = _flashAlpha;
        _hitFlash.color = color;

        //StartCoroutine(Flash_Coroutine());
        StartCoroutine(FlashFadeOut_Coroutine());
    }

    public IEnumerator FlashFadeOut_Coroutine()
    {
        // 1. show that alpha value is 0.4
        Color color = _hitFlash.color;
        float timer = 0f;
        float alpha;

        // 2. implement disappear by changing alpha value to zero
        while (timer < _hitEffectDuration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(_flashAlpha, 0f, timer / _hitEffectDuration);
            color.a = alpha;
            _hitFlash.color = color;

            yield return null;
        }
    }

    private IEnumerator Flash_Coroutine()
    {
        _hitFlash.enabled = true;
        yield return new WaitForSeconds(_hitEffectDuration);
        _hitFlash.enabled = false;
    }
}