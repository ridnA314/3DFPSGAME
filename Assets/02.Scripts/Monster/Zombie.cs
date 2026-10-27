using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Finite State Machine(FSM) : there exist finite state, and act different action by each state
// - state : that object has condition or situation
// - transition : current state trans to other state by condition
// - and setting rule is Machine by using above method
// Each state's behavior lives in its own class (see Monster/State/Zombie*State.cs)
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

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] private ZombieType _type;
    public ZombieType Type => _type;

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

    public float MaxHealth => _maxHealth;
    private float _health;
    public float Health => _health;

    public float DetectionRange => _detectionRange;
    public float ComebackRange => _comebackRange;
    public float AttackDamage => _attackDamage;
    public float AttackRange => _attackRange;
    public float AttackSpeed => _attackSpeed;
    public float StunTime => _stunTime;
    public Vector3 PatrolTargetPosition => _patrolTargetPosition;
    public float PatrolWaitingTime => _patrolWaitingTime;

    public Vector3 StartPosition { get; private set; }
    public GameObject Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private Dictionary<ZombieState, ZombieStateBase> _states;
    private ZombieStateBase _currentState;
    private ZombieState _state;

    public event Action OnDamaged;

    private void Awake()
    {
        _health = _maxHealth;

        _states = new Dictionary<ZombieState, ZombieStateBase>
        {
            { ZombieState.Idle, new ZombieIdleState(this) },
            { ZombieState.Trace, new ZombieTraceState(this) },
            { ZombieState.Comeback, new ZombieComebackState(this) },
            { ZombieState.Attack, new ZombieAttackState(this) },
            { ZombieState.Hit, new ZombieHitState(this) },
            { ZombieState.Death, new ZombieDeathState(this) },
            { ZombieState.Patrol, new ZombiePatrolState(this) },
        };
    }

    private void Start()
    {
        StartPosition = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");

        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = _moveSpeedScalar;

        _state = ZombieState.Idle;
        _currentState = _states[_state];
        _currentState.Enter();

        GameManager.Instance?.IncreaseEnemy();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        _currentState?.Tick();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (damageInfo.Value < 0f) return;
        if (_state == ZombieState.Death) return;

        _health -= damageInfo.Value;
        OnDamaged?.Invoke();

        if (_health > 0f)
        {
            GameObject bloodSpray = Instantiate(_bloodSprayPrefab, transform);
            bloodSpray.transform.position = damageInfo.HitPoint;
            bloodSpray.transform.forward = damageInfo.HitNormal;

            // stun a moment
            ChangeState(ZombieState.Hit);
        }
        else
        {
            GameManager.Instance?.DecreaseEnemy();
            ChangeState(ZombieState.Death);
        }
    }

    public void ChangeState(ZombieState next)
    {
        Debug.Log($"{_state} -> {next}");

        _currentState?.Exit();
        _state = next;
        _currentState = _states[next];
        _currentState.Enter();
    }

    public void StopAgentMove()
    {
        Agent.ResetPath();
        Agent.velocity = Vector3.zero;
    }
}
