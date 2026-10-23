using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaRecoveryPerSecond = 15f;

    public float MaxHealth => _maxHealth;
    public float MaxStamina => _maxStamina;
    public float StaminaRecoveryPerSecond => _staminaRecoveryPerSecond;

    private float _currentHealth;
    public float Health => _currentHealth;
    private float _currentStamina;
    public float Stamina => _currentStamina;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;
    }

    public void UseStamina(float amount)
    {
        if (amount < 0) return;
        if (_currentStamina <= 0) return;
        _currentStamina -= amount;
        UIManager.Instance?.RefreshPlayerStaminaBar();
    }

    public void HealStamina(float amount)
    {
        if (amount < 0) return;
        _currentStamina += amount;

        if (_currentStamina > _maxStamina)
        {
            _currentStamina = _maxStamina;
        }

        UIManager.Instance?.RefreshPlayerStaminaBar();
    }

    public void ApplyDamage(float damage)
    {
        if (damage < 0) return;
        if (_currentStamina <= 0) return;
        _currentHealth -= damage;
        UIManager.Instance?.RefreshPlayerHealthBar();
    }

    public void HealHealth(float amount)
    {
        if (amount < 0) return;
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        UIManager.Instance?.RefreshPlayerHealthBar();
    }
}