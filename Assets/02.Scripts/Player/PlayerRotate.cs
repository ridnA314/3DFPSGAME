using System;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private float _accumulationX = 0f;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (Cursor.visible) return;
        if (!GameManager.Instance.IsPlaying()) return;
        if (!Input.GetMouseButton(2)) return;

        float mouseX = Input.GetAxis("Mouse X");
        _accumulationX += mouseX * GameConfig.RotationSpeedScalar * Time.deltaTime;

        transform.eulerAngles = new Vector3(0f, _accumulationX, 0f);
    }
}