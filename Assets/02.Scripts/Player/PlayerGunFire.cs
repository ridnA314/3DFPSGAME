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

    private const float _fireInterval = 1f;
    private float _timer;

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
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
    }
}