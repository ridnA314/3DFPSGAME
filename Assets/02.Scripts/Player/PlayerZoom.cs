using UnityEngine;

public class PlayerZoom : MonoBehaviour
{
    [SerializeField] private GameObject _sniperZoom;
    [SerializeField] private float _normalPov = 60f;
    [SerializeField] private float _sniperPov = 15f;

    private PlayerStat _playerStat;

    private void Awake()
    {
        _playerStat = GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if (GameManager.Instance?.State != GameState.Playing) return;

        bool isZooming = _playerStat.WeaponMode == WeaponMode.Sniper && Input.GetMouseButton(1);
        _sniperZoom.SetActive(isZooming);
        Camera.main.fieldOfView = isZooming ? _sniperPov : _normalPov;
    }
}