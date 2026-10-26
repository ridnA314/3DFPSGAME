using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat _playerStat;

    public event Action<float> OnDamaged;
    public event Action PlayEffect;

    private void Awake()
    {
        if (_playerStat == null)
        {
            _playerStat = GetComponent<PlayerStat>();
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_playerStat.Health <= 0) return;

        //ScreenEffectManager.Instance?.ShowFlash();
        OnDamaged?.Invoke(damageInfo.Value);
        PlayEffect?.Invoke();
        //_playerStat.ApplyDamage(damageInfo.Value);

        if (_playerStat.Health <= 0)
        {
            Debug.Log("Player Defeated!");
            GameManager.Instance?.GameOver();
        }
    }
}