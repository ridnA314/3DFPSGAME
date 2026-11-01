using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private PlayerStat _playerStat;
    private Animator _animator;

    [SerializeField] private Collider _katanaCollider;
    [SerializeField] private GameObject _sword;

    [SerializeField] private MeleeHitMode _hitMode = MeleeHitMode.Collider; // 맞힐 대상을 고르는 방법
    [SerializeField] private float _fanDamage = 25f; // 부채꼴 방법의 대미지
    [SerializeField] private float _range = 2f; // 부채꼴의 반지름 (m)
    [SerializeField, Range(0f, 360f)] private float _angle = 90f; // 부채꼴 각도 (도)
    [SerializeField] private LayerMask _targetMask; // 때릴 대상 (Drum, Zombie)
    [SerializeField] private LayerMask _obstacleMask; // 가로막는 것 (Default)

    private const string MeleeAttackAnimationKey = "Slash";

    private void Awake()
    {
        _playerStat = GetComponentInParent<PlayerStat>();
        _animator = GetComponentInChildren<Animator>();

        if (_katanaCollider == null)
        {
            _katanaCollider = GetComponentInChildren<Collider>();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        if (_playerStat.WeaponMode != WeaponMode.Melee) return;

        if (Input.GetButtonDown("Fire2"))
        {
            _animator.SetTrigger(MeleeAttackAnimationKey);
        }
    }

    private void OnEnable()
    {
        _playerStat.OnWeaponModeChanged += RefreshSword;
    }

    private void OnDisable()
    {
        _playerStat.OnWeaponModeChanged -= RefreshSword;
    }

    private void Start()
    {
        RefreshSword(_playerStat.WeaponMode);
    }

    private void RefreshSword(WeaponMode mode)
    {
        _sword.SetActive(mode == WeaponMode.Melee);
    }

    public void ActivateCollider()
    {
        if (_hitMode != MeleeHitMode.Collider) return;

        Debug.Log("Activate blade collider");
        _katanaCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        Debug.Log("Deactivate blade collider");
        _katanaCollider.enabled = false;
    }

    // 앞쪽 부채꼴 안의 대상을 한 번에 때린다. slash8의 FanAttack 이벤트가 불러 준다.
    public void FanAttack()
    {
        // 칼날 콜라이더 방법일 때는 하지 않는다.
        if (_hitMode != MeleeHitMode.Fan) return;

        // Goku의 발밑이 아니라 Player의 가운데에서, Player의 앞쪽으로 잰다.
        Transform player = _playerStat.gameObject.transform;
        Vector3 origin = player.position;

        // 1. 사거리 안의 대상을 모두 찾는다.
        Collider[] colliders = Physics.OverlapSphere(origin, _range, _targetMask);
        foreach (Collider col in colliders)
        {
            // 2. 대상에서 플레이어에 가장 가까운 점을 맞을 자리로 삼는다.
            Vector3 hitPoint = col.ClosestPoint(origin);

            // 3. 높이를 뺀 방향이 앞쪽에서 Angle의 절반 안에 있는지 본다.
            Vector3 direction = hitPoint - origin;
            direction.y = 0f;
            if (Vector3.Angle(player.forward, direction) > _angle / 2f) continue;

            // 4. 가는 길에 벽이 있으면 때리지 않는다.
            if (Physics.Linecast(origin, hitPoint, _obstacleMask)) continue;

            // 5. 대미지를 받을 수 있으면 대미지를 준다.
            if (col.TryGetComponent(out IDamageable damageable))
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.Value = _fanDamage;
                damageInfo.HitPoint = hitPoint;
                damageInfo.HitNormal = (origin - hitPoint).normalized;
                damageable.TakeDamage(damageInfo);
            }
        }
    }

    // Goku를 고르면 Scene 창에 부채꼴 방법의 공격 범위를 그린다.
    private void OnDrawGizmosSelected()
    {
        // 플레이하지 않을 때도 불리므로, Awake에서 찾는 _player 대신 여기서 찾는다.
        Player player = GetComponentInParent<Player>();
        if (player == null) return;

        Vector3 origin = player.transform.position;
        Vector3 forward = player.transform.forward;
        Gizmos.color = Color.yellow;

        // 앞쪽을 y축으로 -Angle/2, +Angle/2만큼 돌린 두 방향이 부채꼴의 양 끝이다.
        Vector3 left = Quaternion.Euler(0f, -_angle / 2f, 0f) * forward;
        Vector3 right = Quaternion.Euler(0f, _angle / 2f, 0f) * forward;
        Gizmos.DrawLine(origin, origin + left * _range);
        Gizmos.DrawLine(origin, origin + right * _range);

        // 호: 10도마다 짧은 선을 이어 그린다.
        int count = Mathf.Max(1, Mathf.CeilToInt(_angle / 10f));
        Vector3 previous = origin + left * _range;
        for (int i = 1; i <= count; i++)
        {
            float angle = -_angle / 2f + _angle * i / count;
            Vector3 next = origin + Quaternion.Euler(0f, angle, 0f) * forward * _range;
            Gizmos.DrawLine(previous, next);
            previous = next;
        }
    }
}