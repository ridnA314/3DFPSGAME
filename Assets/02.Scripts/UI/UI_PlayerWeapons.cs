using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerWeapons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _bombText;

    [SerializeField] private Slider _bombChargeGauge;

    [SerializeField] private PlayerGunFire _playerGunFire;
    [SerializeField] private PlayerBombFire _playerBombFire;
    [SerializeField] private PlayerStat _playerStat;

    [SerializeField] private List<WeaponEntry> _weaponEntryList;
    [SerializeField] private Image _weaponIcon;

    private const string AmmoText = "Ammo : {0} / {1}";
    private const string BombText = "Bomb : {0} / {1}";

    private Dictionary<WeaponMode, Sprite> _weaponIconDictionary;

    private void Awake()
    {
        _playerGunFire.Fired += RefreshAmmo;
        _playerBombFire.Throwed += RefreshBomb;
        _playerBombFire.Charging += RefreshBombGauge;
        _playerStat.ChangedWeapon += RefreshWeaponIcon;
    }

    private void Start()
    {
        float startTime = GameManager.Instance.StartTime;
        Invoke("RefreshAmmo", startTime);
        Invoke("RefreshBomb", startTime);

        _weaponIconDictionary = new Dictionary<WeaponMode, Sprite>();
        foreach (WeaponEntry entry in _weaponEntryList)
        {
            _weaponIconDictionary[entry.Mode] = entry.IconImage;
        }
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
        _bombChargeGauge.value = _playerBombFire.ChargeGauge / _playerBombFire.MaxChargeGauge;
    }

    public void RefreshWeaponIcon()
    {
        _weaponIcon.sprite = _weaponIconDictionary[_playerStat.WeaponMode];
    }
}