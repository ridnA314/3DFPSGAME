using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _bombText;

    [SerializeField] private PlayerGunFire _playerGunFire;
    [SerializeField] private PlayerBombFire _playerBombFire;
    [SerializeField] private PlayerStat _playerStat;

    [SerializeField] private Image _bombChargeGauge;
    [SerializeField] private Slider _playerHealthBar;
    [SerializeField] private Slider _playerStaminaBar;

    private const string AmmoText = "Ammo : {0} / {1}";
    private const string BombText = "Bomb : {0} / {1}";

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        RefreshAmmo();
        RefreshBomb();
        RefreshPlayerHealthBar();
        RefreshPlayerStaminaBar();
    }

    public void RefreshAmmo()
    {
        _ammoText.text = string.Format(AmmoText, _playerGunFire.RemainingAmmo, _playerGunFire.MagazineSize);
    }

    public void RefreshBomb()
    {
        _bombText.text = string.Format(BombText, _playerBombFire.RemainingBomb, _playerBombFire.MaxBomb);
    }

    public void RefreshBombGauge()
    {
        _bombChargeGauge.fillAmount = _playerBombFire.ChargeGauge / _playerBombFire.MaxChargeGauge;
    }

    public void RefreshPlayerHealthBar()
    {
        _playerHealthBar.value = _playerStat.Health / _playerStat.MaxHealth;
    }

    public void RefreshPlayerStaminaBar()
    {
        _playerStaminaBar.value = _playerStat.Stamina / _playerStat.MaxStamina;
    }
}