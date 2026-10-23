using System.Collections;
using UnityEngine;

//mouse left button -> fire at center
public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffectPrefab;

    [Header("Stat")]
    [SerializeField] private float _fireRate = 10f;
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private int _magazineSize = 30;
    public int MagazineSize => _magazineSize;
    [SerializeField] private float _reloadSpeed = 3f;

    private int _remainingAmmo;
    public int RemainingAmmo => _remainingAmmo;

    private const float _fireInterval = 1f;
    private const float _reloadTime = 10f;
    private float _timer;
    private Coroutine _reloadCoroutine;

    private void Start()
    {
        _timer = 0f;
        _remainingAmmo = _magazineSize;
        UIManager.Instance?.RefreshAmmo();
    }

    private void Update()
    {
        _timer += Time.deltaTime * _fireRate;
        //1. mouse left click
        if (Input.GetButton("Fire1") && _timer >= _fireInterval && _remainingAmmo > 0)
        {
            Shoot();
            _timer = 0f;
            _remainingAmmo--;
            UIManager.Instance?.RefreshAmmo();

            if (_reloadCoroutine != null)
            {
                GameLogger.Log("Cancel Reload");
                StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
            }
        }

        if (Input.GetButtonDown("Reload") && _reloadCoroutine == null)
        {
            _reloadCoroutine = StartCoroutine(Reloading_Coroutine());
        }
    }

    private IEnumerator Reloading_Coroutine()
    {
        GameLogger.Log("Start Reload");
        yield return new WaitForSeconds(_reloadTime / _reloadSpeed);

        _remainingAmmo = _magazineSize;
        UIManager.Instance?.RefreshAmmo();
        GameLogger.Log("Reload Complete");
        _reloadCoroutine = null;
    }

    private void Shoot()
    {
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
            //play effect
            ParticleSystem effect = Instantiate(_hitEffectPrefab);
            effect.transform.position = hitInfo.point; // hit position
            effect.transform.forward = hitInfo.normal;
            effect.Play();

            DamageInfo damageInfo = new DamageInfo();
            damageInfo.Value = _damage;
            damageInfo.HitPoint = hitInfo.point;
            damageInfo.HitNormal = hitInfo.normal;

            //If target is Zombie, call TakeDamage of Zombie
            hitInfo.collider.GetComponent<IDamageable>()?.TakeDamage(damageInfo);
        }
    }
}