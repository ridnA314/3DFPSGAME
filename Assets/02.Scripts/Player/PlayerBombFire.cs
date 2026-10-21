using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _throwPower = 15f;
    [SerializeField] private int _maxBomb = 10;
    public int MaxBomb => _maxBomb;

    private int _remainingBomb;
    public int RemainingBomb => _remainingBomb;

    private void Start()
    {
        _remainingBomb = _maxBomb;
        UIManager.Instance?.RefreshBomb();
    }

    private void Update()
    {
        //mouse right click
        if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_remainingBomb <= 0) return;

        Bomb bomb = Instantiate(_bombPrefab, _firePoint.position, Quaternion.identity);

        Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
        rigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);

        _remainingBomb -= 1;
        UIManager.Instance?.RefreshBomb();
    }
}