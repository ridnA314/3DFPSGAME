using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaRecoveryPerSecond = 15f;

    private Player _player;

    public float MaxHealth => _maxHealth;
    public float MaxStamina => _maxStamina;
    public float StaminaRecoveryPerSecond => _staminaRecoveryPerSecond;

    private float _currentHealth;
    public float Health => _currentHealth;
    private float _currentStamina;
    public float Stamina => _currentStamina;

    private WeaponMode _weaponMode;
    public WeaponMode WeaponMode => _weaponMode;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;

        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.OnDamaged += ApplyDamage;
    }

    public void UseStamina(float amount)
    {
        if (amount < 0) return;
        if (_currentStamina <= 0) return;

        _currentStamina = Mathf.Clamp(_currentStamina - amount, 0, _maxStamina);
        UIManager.Instance?.RefreshPlayerStaminaBar();
    }

    public void HealStamina(float amount)
    {
        if (amount < 0) return;

        _currentStamina = Mathf.Clamp(_currentStamina + amount, 0, _maxStamina);
        UIManager.Instance?.RefreshPlayerStaminaBar();
    }

    private void ApplyDamage(float damage)
    {
        if (damage < 0) return;
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        UIManager.Instance?.RefreshPlayerHealthBar();
    }

    public void HealHealth(float amount)
    {
        if (amount < 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        UIManager.Instance?.RefreshPlayerHealthBar();
    }

    public void LeftChangeMode()
    {
        int changeMode = (int)_weaponMode - 1;
        int count = Enum.GetNames(typeof(WeaponMode)).Length - 1;
        if (changeMode < 0)
        {
            changeMode = count;
        }

        _weaponMode = (WeaponMode)changeMode;
    }

    public void RightChangeMode()
    {
        int changeMode = (int)_weaponMode + 1;
        int count = Enum.GetNames(typeof(WeaponMode)).Length - 1;
        if (changeMode > count)
        {
            changeMode = 0;
        }

        _weaponMode = (WeaponMode)changeMode;
    }
}