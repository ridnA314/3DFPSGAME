using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat _playerStat;

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

        ScreenEffectManager.Instance?.ShowFlash();
        _playerStat.ApplyDamage(damageInfo.Value);
        Debug.Log($"Player Hit!! {damageInfo.Value}");

        if (_playerStat.Health <= 0)
        {
            Debug.Log("Player Defeated!");
            //ToDo. Game Over
        }
    }
}