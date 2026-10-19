using UnityEngine;

//Role : Rotate Camera when move mouse
public class FpsCameraRotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeedScalar = 200f;

    private float _accumulationX = 0f; //left/right angle
    private float _accumulationY = 0f; //up/down angle

    private void Update()
    {
        //1. Input mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //2. calculate rotate direction by using mouse input
        //Vector3 direction = new Vector3(-mouseY, mouseX, 0f);
        // stock rotation angle by using mouse input
        _accumulationX += mouseX * _rotationSpeedScalar * Time.deltaTime;
        _accumulationY += -mouseY * _rotationSpeedScalar * Time.deltaTime;

        //3. limit rotation angle
        _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);

        //4.Apply stock at Camera
        transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0f);

        /*
        //3. rotate above direction
        // new angle = current angle + angle speed * delta time
        transform.eulerAngles += _rotationSpeedScalar * Time.deltaTime * direction;

        //4. limit up/down rotation range -> -90~90
        //but unity use 0~360 structure -> minus value change to plus value(360-value) by auto
        Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Max(-90, Mathf.Min(90, rotation.x));
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);
        transform.eulerAngles = rotation;
        */
    }
}