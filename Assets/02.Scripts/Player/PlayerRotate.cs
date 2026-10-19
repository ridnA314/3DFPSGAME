using System;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private float _accumulationX = 0f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _accumulationX += mouseX * GameConfig.RotationSpeedScalar * Time.deltaTime;

        transform.eulerAngles = new Vector3(0f, _accumulationX, 0f);
    }
}