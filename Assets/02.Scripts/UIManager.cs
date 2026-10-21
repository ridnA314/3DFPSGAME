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
    [SerializeField] private Image _bombChargeGauge;

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
}