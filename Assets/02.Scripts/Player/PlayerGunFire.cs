using UnityEngine;

//mouse left button -> fire at center
public class PlayerGunFire : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private ParticleSystem _hitEffectPrefab;
    [SerializeField] private float _fireRate = 10f;
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private int _magazineSize = 50;
    public int MagazineSize => _magazineSize;
    [SerializeField] private float _reloadSpeed = 2f;

    private int _remainingAmmo;
    public int RemainingAmmo => _remainingAmmo;
    private int _currentAmmo;
    private bool _isReloading = false;

    private const float _fireInterval = 1f;
    private float _timer;

    private const float _reloadTime = 1f;
    private float _reloadTimer;

    private void Start()
    {
        _timer = 0f;
        _reloadTimer = 0f;
        _remainingAmmo = _magazineSize;
        UIManager.Instance?.RefreshAmmo();
    }

    private void Update()
    {
        TryReload();
        Reloading();

        _timer += Time.deltaTime * _fireRate;
        //1. mouse left click
        if (Input.GetButton("Fire1") && _timer >= _fireInterval)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void Shoot()
    {
        if (_isReloading && !CheckRemainingAmmo()) return;

        //2. create lay, set fire position and direction
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        //3. create ray hit target variable
        RaycastHit hitInfo = new RaycastHit();

        //4. fire ray and diplay effect when hit
        bool isHit = Physics.Raycast(ray, out hitInfo, _range);
        if (isHit)
        {
            //GameLogger.Log($"Hit {hitInfo.collider.name} by bullet, damage: {_damage}");
            ParticleSystem effect = Instantiate(_hitEffectPrefab);
            effect.transform.position = hitInfo.point; // hit position
            effect.transform.forward = hitInfo.normal;
            effect.Play();
        }

        UseAmmo();
    }

    private void Reloading()
    {
        if (!_isReloading) return;

        _reloadTimer += Time.deltaTime * _reloadSpeed;
        _remainingAmmo = _currentAmmo + (int)((_magazineSize - _currentAmmo) * (_reloadTimer / _reloadTime));
        if (_remainingAmmo >= _magazineSize)
        {
            _remainingAmmo = _magazineSize;
        }

        if (_reloadTimer >= _reloadTime)
        {
            _isReloading = false;
        }

        UIManager.Instance?.RefreshAmmo();
    }

    private void TryReload()
    {
        if (Input.GetButtonDown("Reload") && !_isReloading)
        {
            _isReloading = true;
            _currentAmmo = _remainingAmmo;
            _reloadTimer = _reloadTime * ((float)_currentAmmo / _magazineSize);
        }
    }

    private bool CheckRemainingAmmo()
    {
        return _remainingAmmo > 0;
    }

    private void UseAmmo()
    {
        --_remainingAmmo;
        UIManager.Instance?.RefreshAmmo();
    }
}