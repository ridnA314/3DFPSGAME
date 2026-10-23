using UnityEngine;

//Role : Rotate Camera when move mouse
public class CameraRotate : MonoBehaviour
{
    private float _accumulationX = 0f; //left/right angle
    private float _accumulationY = 0f; //up/down angle

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        Rotate();
    }

    private void Rotate()
    {
        if (!Input.GetMouseButton(2)) return;

        //1. Input mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //2. calculate rotate direction by using mouse input
        // stock rotation angle by using mouse input
        _accumulationX += mouseX * GameConfig.RotationSpeedScalar * Time.deltaTime;
        _accumulationY += -mouseY * GameConfig.RotationSpeedScalar * Time.deltaTime;

        //3. limit rotation angle
        _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);

        //4.Apply stock at Camera
        transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0f);
    }
}