using System.Collections;
using UnityEngine;

// explosion when health become zero
[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Stat")]
    [SerializeField] private float _health = 30f;
    [SerializeField] private float _launchPower = 20f;
    [SerializeField] private float _spinPower = 5f;
    [SerializeField] private float _destroyDelay = 3f;
    [SerializeField] private float _explosionDelay = 0.1f;
    [SerializeField] private float _explosionRange = 4.5f;
    [SerializeField] private float _explosionDamage = 100f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (damageInfo.Value <= 0) return;
        if (_health <= 0) return;

        _health -= damageInfo.Value;

        if (_health <= 0)
        {
            StartCoroutine(Explode_Coroutine());
        }
    }

    private IEnumerator Explode_Coroutine()
    {
        // 0. wait before explosion
        yield return new WaitForSeconds(_explosionDelay);

        // 1. spawn explosion effect
        ParticleSystem effect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        effect.Play();

        // 2. spring up and spin
        _rigidbody.AddForce(Vector3.up * _launchPower, ForceMode.Impulse);
        _rigidbody.AddTorque(UnityEngine.Random.insideUnitSphere * _spinPower, ForceMode.Impulse);

        //3. send explosion to around
        ApplyAreaDamage();

        // after delay, destroy slef
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(gameObject);
    }

    private void ApplyAreaDamage()
    {
        // 0. find all Drum at around
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            _explosionRange,
            _targetLayer);

        // 1. Give Damage them
        foreach (Collider col in colliders)
        {
            DamageInfo damageInfo = new()
            {
                Value = _explosionDamage,
                HitPoint = col.ClosestPoint(transform.position),
                HitNormal = (transform.position - col.ClosestPoint(transform.position)).normalized,
            };

            col.GetComponent<IDamageable>()?.TakeDamage(damageInfo);
        }
    }
}