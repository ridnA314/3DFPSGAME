using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
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

    private void Start()
    {
        _chargeGauge = 0f;
        _remainingBomb = _maxBomb;
        UIManager.Instance?.RefreshBomb();
        UIManager.Instance?.RefreshBombGauge();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _isCharging = true;
        }

        Charge();

        if (Input.GetButtonUp("Fire2"))
        {
            _isCharging = false;
            Shoot();
            _chargeGauge = 0f;
        }
    }

    private void Shoot()
    {
        if (_remainingBomb <= 0) return;

        Bomb bomb = Instantiate(_bombPrefab, _firePoint.position, Quaternion.identity);

        Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
        rigidbody.AddForce(Camera.main.transform.forward * (_throwPower * _chargeGauge), ForceMode.Impulse);

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