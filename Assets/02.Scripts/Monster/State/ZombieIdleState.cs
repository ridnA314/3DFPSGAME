using UnityEngine;

// just wait for player at current position
public class ZombieIdleState : ZombieStateBase
{
    private const float PatrolArrivalDistance = 0.5f;

    private float _patrolTimer;

    public ZombieIdleState(Zombie zombie) : base(zombie) { }

    public override void Enter()
    {
        _patrolTimer = 0f;
        Zombie.StopAgentMove();
    }

    public override void Tick()
    {
        Zombie.StopAgentMove();

        _patrolTimer += Time.deltaTime;
        if (_patrolTimer >= Zombie.PatrolWaitingTime &&
            Vector3.Distance(Zombie.transform.position, Zombie.PatrolTargetPosition) <= PatrolArrivalDistance)
        {
            Zombie.ChangeState(ZombieState.Comeback);
            return;
        }

        if (_patrolTimer >= Zombie.PatrolWaitingTime)
        {
            Zombie.ChangeState(ZombieState.Patrol);
            return;
        }

        //ToDo. Play Idle animation
        //when close to player, trans to Trace State
        if (Vector3.Distance(Zombie.transform.position, Zombie.Player.transform.position) <= Zombie.DetectionRange)
        {
            Zombie.ChangeState(ZombieState.Trace);
        }
    }
}
