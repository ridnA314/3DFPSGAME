using System;
using System.Collections;
using UnityEngine;

//Finite State Machine(FSM) : there exist finite state, and act different action by each state
// - state : that object has condition or situation
// - transition : current state trans to other state by condition
// - and setting rule is Machine by using above method
public enum ZombieState
{
    Idle, // waiting : just wait for player at own place
    Trace, // chase : chase to plyer
    Comeback, // Return : return to initial position
    Attack, // Attack : attack by certain interval
    Hit, // Hit : attacked and then stun just moment
    Death, // Dead : lay on ground, and do not act
    Patrol, // Patrol : patrol certain distance
}

public class Zombie : MonoBehaviour, IDamageable
{
    [Header("Stat")]
    [SerializeField] private float _detectionRange = 8f; // distance that find player
    [SerializeField] private float _comebackRange = 20f; // comeback when too far from first position
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackSpeed = 8f;
    [SerializeField] private float _stunTime = 0.3f;
    [SerializeField] private float _moveSpeedScalar = 5f;
    [SerializeField] private float _maxHealth = 30f;
    [SerializeField] private Vector3 _patrolTargetPosition;
    [SerializeField] private float _patrolWaitingTime = 3f;

    [Header("Effect")]
    [SerializeField] private GameObject _bloodSprayPrefab;

    private const float AttackInterval = 10f;

    public float MaxHealth => _maxHealth;
    private float _health;
    public float Health => _health;

    private float _patrolTimer;
    private float _attackTimer;
    private CharacterController _controller;
    private Vector3 _startPosition;
    private GameObject _player;

    private ZombieState _state;

    private void Start()
    {
        _health = _maxHealth;
        _patrolTimer = 0f;
        _attackTimer = 0f;

        _controller = GetComponent<CharacterController>();
        _startPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player");

        _state = ZombieState.Idle;
    }

    private void Update()
    {
        switch (_state)
        {
            case ZombieState.Idle:
                Idle();
                //ToDO : wait : just wait for player at current position
                break;
            case ZombieState.Trace:
                Trace();
                //ToDO : chase to Player
                break;
            case ZombieState.Comeback:
                Comeback();
                break;
            case ZombieState.Attack:
                Attack();
                break;
            case ZombieState.Patrol:
                Patrol();
                break;
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (damageInfo.Value < 0f) return;
        if (_state == ZombieState.Death) return;

        _health -= damageInfo.Value;

        StopAllCoroutines();

        if (_health > 0f)
        {
            GameObject bloodSpray = Instantiate(_bloodSprayPrefab, transform);
            bloodSpray.transform.position = damageInfo.HitPoint;
            bloodSpray.transform.forward = damageInfo.HitNormal;

            // stun a moment
            Debug.Log($"{_state} -> {ZombieState.Hit}");
            _state = ZombieState.Hit;
            StartCoroutine(Hit_Coroutine());
        }
        else
        {
            Debug.Log($"{_state} -> {ZombieState.Death}");
            _state = ZombieState.Death;
            StartCoroutine(Death_Coroutine());
        }
    }

    private IEnumerator Hit_Coroutine()
    {
        yield return new WaitForSeconds(_stunTime);

        // Trace because attacked
        Debug.Log($"{_state} -> {ZombieState.Trace}");
        _state = ZombieState.Trace;
    }

    private IEnumerator Death_Coroutine()
    {
        _controller.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // just wait
    private void Idle()
    {
        _patrolTimer += Time.deltaTime;
        if (_patrolTimer >= _patrolWaitingTime &&
            Vector3.Distance(transform.position, _patrolTargetPosition) <= 0.5f)
        {
            Debug.Log($"{_state} -> {ZombieState.Comeback}");
            _state = ZombieState.Comeback;
            return;
        }

        if (_patrolTimer >= _patrolWaitingTime)
        {
            Debug.Log($"{_state} -> {ZombieState.Patrol}");
            _patrolTimer = 0f;
            _state = ZombieState.Patrol;
            return;
        }

        //ToDo. Play Idle animation
        //when close to player, trans to Trace State
        float distanceThreshold = Vector3.Distance(transform.position, _player.transform.position);
        if (distanceThreshold <= _detectionRange)
        {
            Debug.Log($"{_state} -> {ZombieState.Trace}");
            _state = ZombieState.Trace;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _patrolTargetPosition) < 0.5f)
        {
            ChangeToIdle();
            return;
        }

        //ToDo. Play Walk animation
        Vector3 direction = _patrolTargetPosition - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.forward = direction;
        _controller.SimpleMove(direction * _moveSpeedScalar);
    }

    private void Trace()
    {
        //ToDo. Play Run animation

        // too fat from first position
        if (Vector3.Distance(transform.position, _startPosition) > _comebackRange)
        {
            Debug.Log($"{_state} -> {ZombieState.Comeback}");
            _state = ZombieState.Comeback;
            return;
        }

        // Attack when distance self and player is less than attack range
        if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
        {
            _attackTimer = 0f;
            Debug.Log($"{_state} -> {ZombieState.Attack}");
            _state = ZombieState.Attack;
            return;
        }

        Vector3 direction = _player.transform.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.forward = direction;
        _controller.SimpleMove(direction * _moveSpeedScalar);
    }

    private void Comeback()
    {
        // When achieve at first position, trans to Idle State
        if (Vector3.Distance(transform.position, _startPosition) <= 0.5f)
        {
            ChangeToIdle();
            return;
        }

        Vector3 direction = _startPosition - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.forward = direction;
        _controller.SimpleMove(direction * _moveSpeedScalar);
    }

    // Attack : attack per certain interval
    private void Attack()
    {
        // ditance to player greater than attack range
        float distanceThreshold = Vector3.Distance(transform.position, _player.transform.position);
        if (distanceThreshold > _attackRange)
        {
            Debug.Log($"{_state} -> {ZombieState.Trace}");
            _state = ZombieState.Trace;
        }

        _attackTimer += Time.deltaTime * _attackSpeed;
        if (_attackTimer >= AttackInterval)
        {
            //ToDo. Play Attack animation
            DamageInfo info = new DamageInfo()
            {
                Value = _attackDamage,
                HitPoint = _player.transform.position,
                HitNormal = (transform.position - _player.transform.position).normalized,
            };

            _player.GetComponent<Player>()?.TakeDamage(info);
            _attackTimer = 0f;
        }
    }

    private void ChangeToIdle()
    {
        Debug.Log($"{_state} -> {ZombieState.Idle}");
        _patrolTimer = 0f;
        _state = ZombieState.Idle;
    }
}