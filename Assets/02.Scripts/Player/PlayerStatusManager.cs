using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    private static PlayerStatusManager _instance;
    public static PlayerStatusManager Instance => _instance;

    [SerializeField] private PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus => _playerStatus;
    private bool _isUseStamina;

    [SerializeField] private float _healStaminaPerSecond;
    [SerializeField] private float _useRunningStamiaPerSecond;
    [SerializeField] private float _useJumpStamia;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        if (_playerStatus != null)
        {
            _playerStatus.Init();
        }
    }

    private void Start()
    {
        _isUseStamina = false;
    }

    private void Update()
    {
        if (!_isUseStamina)
        {
            IncreaseStamina(_healStaminaPerSecond * Time.deltaTime);
        }
    }

    private void DecreaseStamina(float amount)
    {
        if (amount <= 0) return;
        _playerStatus.Stamina -= amount;
    }

    private void IncreaseStamina(float amount)
    {
        if (amount <= 0) return;
        _playerStatus.Stamina += amount;
    }

    public bool IsZeroStamina()
    {
        return _playerStatus.Stamina <= 0;
    }

    public void RunningUseStamina(float deltaTime)
    {
        DecreaseStamina(_useRunningStamiaPerSecond * deltaTime);
    }

    public void JumpUseStamina()
    {
        DecreaseStamina(_useJumpStamia);
    }
}