using System;
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
    Hit, // Hit : attacked and than stun just moment
    Death, // Dead : lay on ground, and do not act
}

public class Zombie : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private float _detectionRange = 8f; // distance that find player
    [SerializeField] private float _moveSpeedScalar = 5f;

    private CharacterController _controller;
    private GameObject _player;

    private ZombieState _state;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
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
            case ZombieState.Hit:
                break;
            case ZombieState.Death:
                break;
        }
    }

    // just wait
    private void Idle()
    {
        //ToDo. Play Idle animation
        //when close to player, trans to Trace State
        float distanceThreshold = Vector3.Distance(transform.position, _player.transform.position);
        if (distanceThreshold <= _detectionRange)
        {
            Debug.Log($"{_state} -> {ZombieState.Trace}");
            _state = ZombieState.Trace;
        }
    }

    private void Trace()
    {
        //ToDo. Play Run animation
        Vector3 direction = _player.transform.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.forward = direction;
        _controller.SimpleMove(direction * _moveSpeedScalar);
    }

    private void Comeback()
    {
    }

    private void Attack()
    {
    }
}