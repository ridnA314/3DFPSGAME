using System;
using UnityEngine;

public class PlayerZoom : MonoBehaviour
{
    private PlayerStat _playerStat;

    private void Awake()
    {
        _playerStat = GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if (GameManager.Instance?.State != GameState.Playing) return;

        bool isZooming = _playerStat.WeaponMode == WeaponMode.Sniper && Input.GetMouseButton(1);
        Camera.main.fieldOfView = isZooming ? 15f : 60f;
    }
}