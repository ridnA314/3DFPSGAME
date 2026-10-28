using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;
    [SerializeField] private float _explosionRange = 4.5f;
    [SerializeField] private float _explosionDamage = 100f;

    [SerializeField] private LayerMask _targetLayer;

    private bool _isExploded;

    private void OnEnable()
    {
        _isExploded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // several contacts can arrive in the same physics step, so explode and release only once
        if (_isExploded) return;
        _isExploded = true;

        ParticleSystem effect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        effect.Play();

        //erase bomb
        //Destroy(gameObject);
        ApplyAreaDamage();
        BombPoolManager.Instance?.Release(this);
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