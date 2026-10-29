using System.Collections;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    private PlayerStat _playerStat;

    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _throwPower = 6f;
    [SerializeField] private int _maxBomb = 10;
    [SerializeField] private float _maxChargeGauge = 3f;

    public int MaxBomb => _maxBomb;
    public float MaxChargeGauge => _maxChargeGauge;

    private int _remainingBomb;
    public int RemainingBomb => _remainingBomb;

    private bool _isCharging;
    private float _chargeGauge;
    public float ChargeGauge => _chargeGauge;

    private const float ThrowClipFrameRate = 30f;
    private const int ThrowReleaseFrame = 55; // release frame of the 96-frame Toss Grenade clip
    private static readonly int ThrowHash = Animator.StringToHash("Throw");
    private Animator _animator;
    private bool _isThrowing;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerStat = GetComponent<PlayerStat>();
    }

    private void Start()
    {
        _chargeGauge = 0f;
        _remainingBomb = _maxBomb;
        UIManager.Instance?.RefreshBomb();
        UIManager.Instance?.RefreshBombGauge();
    }

    private void Update()
    {
        bool isBomb = _playerStat.WeaponMode == WeaponMode.Normal;
        if (Input.GetButtonDown("Fire2") && isBomb)
        {
            _isCharging = true;
        }

        Charge();

        if (Input.GetButtonUp("Fire2") && isBomb)
        {
            _isCharging = false;
            if (!_isThrowing && _remainingBomb > 0)
            {
                StartCoroutine(Throw_Coroutine(_chargeGauge));
            }

            _chargeGauge = 0f;
            UIManager.Instance?.RefreshBombGauge();
        }
    }

    // play throw motion, then release the bomb at the frame where the hand lets go
    private IEnumerator Throw_Coroutine(float chargeGauge)
    {
        _isThrowing = true;
        _animator.SetTrigger(ThrowHash);

        yield return new WaitForSeconds(ThrowReleaseFrame / ThrowClipFrameRate);

        Shoot(chargeGauge);
        _isThrowing = false;
    }

    private void Shoot(float chargeGauge)
    {
        if (_remainingBomb <= 0) return;

        Bomb bomb = BombPoolManager.Instance?.Get();
        if (bomb == null) return;
        bomb.transform.position = _firePoint.position;

        Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(Camera.main.transform.forward * (_throwPower * chargeGauge), ForceMode.Impulse);

        _remainingBomb -= 1;
        UIManager.Instance?.RefreshBomb();
    }

    private void Charge()
    {
        if (!_isCharging) return;

        _chargeGauge += Time.deltaTime;
        if (_chargeGauge >= _maxChargeGauge)
        {
            _chargeGauge = _maxChargeGauge;
        }

        UIManager.Instance?.RefreshBombGauge();
    }
}