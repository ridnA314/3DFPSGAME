using UnityEngine;

// Want to move by keyboard input
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeedScalar = 7f;
    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        //1. input keyboard
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //2. calculate direction by using keyboard input
        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();
        direction = transform.TransformDirection(direction);
        //local coordinate : based on one target
        //world coordinate : based on world

        //3. move by speed and direction
        //transform.position += direction * (_moveSpeedScalar * Time.deltaTime);
        _characterController.Move(direction * (_moveSpeedScalar * Time.deltaTime));
    }
}