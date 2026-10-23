using UnityEngine;

// Want to move by keyboard input
public class PlayerMove : MonoBehaviour
{
    private const float Gravity = -20f;
    private float _yVelocity = 0f; // vertical speed scalar, add per frame
    private int _jumpCount = 0; // count of jump

    [SerializeField] private PlayerStat _playerStat;
    [SerializeField] private float _moveSpeedScalar = 7f;
    [SerializeField] private float _sprintSpeedScalar = 12f;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private int _maxJumpCount = 2;
    [SerializeField] private float _sprintStaminaCost = 10f;
    [SerializeField] private float _jumpStaminaCost = 20f;

    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }

        if (_playerStat == null)
        {
            _playerStat = GetComponent<PlayerStat>();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        //0. recovery stamina
        float healAmount = Mathf.Min(_playerStat.MaxHealth,
                               _playerStat.Stamina + Time.deltaTime * _playerStat.StaminaRecoveryPerSecond) -
                           _playerStat.Stamina;
        _playerStat.HealStamina(healAmount);

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
        if (!_characterController.isGrounded)
        {
            _yVelocity += Gravity * Time.deltaTime;
        }
        else
        {
            _yVelocity = -1f;
            _jumpCount = 0;
        }

        //4. If press jump key, Try Jump
        bool jumpCondition = (_jumpCount >= 1 && _jumpCount < _maxJumpCount) ||
                             (_jumpCount < 1 && _characterController.isGrounded);
        if (Input.GetButtonDown("Jump") && jumpCondition && _playerStat.Stamina >= _jumpStaminaCost)
        {
            _yVelocity = _jumpPower;
            _jumpCount++;
            _playerStat.UseStamina(_jumpStaminaCost);
        }

        //5. calculate velocity
        bool wantSprint = Input.GetButton("Sprint");
        float currentSpeed = _moveSpeedScalar;
        if (wantSprint)
        {
            float needStaminaCost = _sprintStaminaCost * Time.deltaTime;
            if (_playerStat.Stamina >= needStaminaCost)
            {
                currentSpeed = _sprintSpeedScalar;
                _playerStat.UseStamina(needStaminaCost);
            }
        }

        Vector3 velocity = direction * currentSpeed;
        velocity.y = _yVelocity;

        //6. move by velocity
        //transform.position += direction * (_moveSpeedScalar * Time.deltaTime);
        _characterController.Move(velocity * Time.deltaTime);
    }
}