using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat _playerStat;

    private Animator _animator;
    private int _injuredLayerIndex;

    public event Action<float> OnDamaged;
    public event Action PlayEffect;

    private void Awake()
    {
        if (_playerStat == null)
        {
            _playerStat = GetComponent<PlayerStat>();
        }

        _animator = GetComponentInChildren<Animator>();
        _injuredLayerIndex = _animator.GetLayerIndex("Injured Layer");
    }

    private void Update()
    {
        if (GameManager.Instance?.State != GameState.Playing) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerStat.LeftChangeMode();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerStat.RightChangeMode();
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_playerStat.Health <= 0) return;

        //ScreenEffectManager.Instance?.ShowFlash();
        OnDamaged?.Invoke(damageInfo.Value);
        PlayEffect?.Invoke();

        float healthPercentage = _playerStat.Health / _playerStat.MaxHealth; // Real number between with 0 ~ 1
        float injured = Mathf.InverseLerp(0.5f, 0f, healthPercentage);
        GameLogger.Log($"_injuredLayerIndex : {_injuredLayerIndex}");
        _animator.SetLayerWeight(_injuredLayerIndex, injured);
        //_playerStat.ApplyDamage(damageInfo.Value);

        if (_playerStat.Health <= 0)
        {
            Debug.Log("Player Defeated!");
            GameManager.Instance?.GameOver();
        }
    }
}