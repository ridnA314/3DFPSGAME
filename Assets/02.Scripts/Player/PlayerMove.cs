using UnityEngine;

// Want to move by keyboard input
public class PlayerMove : MonoBehaviour
{
    private const float Gravity = -20f;
    private float _yVelocity = 0f; // vertical speed scalar, add per frame
    private const int JumpMaxCount = 2;
    private int _jumpCount = 0;
    private bool _isZeroStamina = false;

    private PlayerStatusManager _statusManager = null;
    private bool _isStatusManagerInitialized = false;

    [SerializeField] private float _moveSpeedScalar = 7f;
    [SerializeField] private float _runningSpeedScalar = 12f;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }

        _statusManager = PlayerStatusManager.Instance;
        if (_statusManager != null)
        {
            _isStatusManagerInitialized = true;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
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

        //3. stack gravity, when is ground
        TakeGravity();

        CheckStamina();

        //4. If press jump key, Try Jump
        Jump();

        //5. calculate velocity
        CheckStamina();

        Vector3 velocity = direction;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isZeroStamina)
        {
            velocity *= _runningSpeedScalar;
            if (_isStatusManagerInitialized)
            {
                _statusManager.RunningUseStamina(Time.deltaTime);
            }
        }
        else
        {
            velocity *= _moveSpeedScalar;
        }

        velocity.y = _yVelocity;

        //6. move by velocity
        //transform.position += direction * (_moveSpeedScalar * Time.deltaTime);
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _jumpCount < JumpMaxCount && !_isZeroStamina)
        {
            _yVelocity = _jumpPower;
            _jumpCount += 1;
            if (_isStatusManagerInitialized)
            {
                _statusManager.JumpUseStamina();
            }
        }
    }

    private void CheckStamina()
    {
        if (_isStatusManagerInitialized)
        {
            _isZeroStamina = _statusManager.IsZeroStamina();
        }
    }

    private void TakeGravity()
    {
        if (!_characterController.isGrounded)
        {
            _yVelocity += Gravity * Time.deltaTime;
        }
        else
        {
            _yVelocity = 0;
            _jumpCount = 0;
        }
    }
}