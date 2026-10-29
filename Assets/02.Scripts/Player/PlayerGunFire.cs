using System;
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

    [SerializeField] private GameObject[] _muzzleFlashes;
    [SerializeField] private float _flashTime = 0.05f;

    [Header("Bullet Tracer")]
    [SerializeField] private LineRenderer _tracerLine;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private Material _tracerMaterial;
    [SerializeField] private Color _tracerColor = new Color(1f, 0.9f, 0.4f, 1f);
    [SerializeField] private float _tracerWidth = 0.03f;
    [SerializeField] private float _tracerDuration = 0.05f;

    private int _remainingAmmo;
    public int RemainingAmmo => _remainingAmmo;

    private const float _fireInterval = 1f;
    private const float _reloadTime = 10f;
    private float _timer;
    private Coroutine _reloadCoroutine;
    private Coroutine _tracerCoroutine;

    private Animator _animator;

    public event Action Fired;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        SetupTracerLine();
    }

    private void Start()
    {
        _timer = 0f;
        _remainingAmmo = _magazineSize;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        _timer += Time.deltaTime * _fireRate;
        //1. mouse left click
        bool isMouseVisible = Cursor.visible;
        if (!isMouseVisible && Input.GetButton("Fire1") && _timer >= _fireInterval && _remainingAmmo > 0)
        {
            _timer = 0f;
            _remainingAmmo--;
            Fired?.Invoke();

            Shoot();
            _animator.SetBool("Fire", true);

            if (_reloadCoroutine != null)
            {
                GameLogger.Log("Cancel Reload");
                StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
            }
        }
        else
        {
            _animator.SetBool("Fire", false);
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
        Fired?.Invoke();
        GameLogger.Log("Reload Complete");
        _reloadCoroutine = null;
    }

    private void Shoot()
    {
        StartCoroutine(MuzzleFlash_Coroutine());

        //2. create lay, set fire position and direction
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        //3. create ray hit target variable
        RaycastHit hitInfo = new RaycastHit();

        //4. fire ray and diplay effect when hit
        bool isHit = Physics.Raycast(ray, out hitInfo, _range);

        //5. draw bullet tracer from muzzle to hit point (or max range)
        Vector3 endPoint = isHit ? hitInfo.point : ray.origin + ray.direction * _range;
        ShowTracer(endPoint);

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

    private void SetupTracerLine()
    {
        if (_tracerLine == null)
        {
            _tracerLine = gameObject.AddComponent<LineRenderer>();
        }

        _tracerLine.positionCount = 2;
        _tracerLine.useWorldSpace = true;
        _tracerLine.startWidth = _tracerWidth;
        _tracerLine.endWidth = _tracerWidth;
        _tracerLine.startColor = _tracerColor;
        _tracerLine.endColor = _tracerColor;
        _tracerLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _tracerLine.receiveShadows = false;
        _tracerLine.sharedMaterial = _tracerMaterial != null ? _tracerMaterial : new Material(Shader.Find("Sprites/Default"));
        _tracerLine.enabled = false;
    }

    private void ShowTracer(Vector3 endPoint)
    {
        Vector3 startPoint = GetMuzzlePosition();
        _tracerLine.SetPosition(0, startPoint);
        _tracerLine.SetPosition(1, endPoint);

        if (_tracerCoroutine != null)
        {
            StopCoroutine(_tracerCoroutine);
        }
        _tracerCoroutine = StartCoroutine(Tracer_Coroutine());
    }

    private Vector3 GetMuzzlePosition()
    {
        if (_muzzlePoint != null) return _muzzlePoint.position;
        if (_muzzleFlashes != null && _muzzleFlashes.Length > 0 && _muzzleFlashes[0] != null)
        {
            return _muzzleFlashes[0].transform.position;
        }
        return Camera.main.transform.position;
    }

    private IEnumerator Tracer_Coroutine()
    {
        _tracerLine.enabled = true;
        yield return new WaitForSeconds(_tracerDuration);
        _tracerLine.enabled = false;
        _tracerCoroutine = null;
    }

    private IEnumerator MuzzleFlash_Coroutine()
    {
        int randomIndex = UnityEngine.Random.Range(0, _muzzleFlashes.Length);
        GameObject flash = _muzzleFlashes[randomIndex];
        flash.SetActive(true);

        yield return new WaitForSeconds(_flashTime);

        flash.SetActive(false);
    }
}