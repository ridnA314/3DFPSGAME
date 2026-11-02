using UnityEngine;

// chase to player
public class ZombieTraceState : ZombieStateBase
{
    // re-requesting the path every frame is wasteful, the player barely moves in 0.2s
    private const float DestinationUpdateInterval = 0.2f;

    private float _destinationTimer;

    public ZombieTraceState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        // update the destination on the first Tick
        _destinationTimer = DestinationUpdateInterval;
    }

    public override void Tick()
    {
        //ToDo. Play Run animation

        // too far from first position
        if (Vector3.Distance(Zombie.transform.position, Zombie.StartPosition) > Zombie.ComebackRange)
        {
            Zombie.Animator.SetTrigger("RunToMove");
            Zombie.ChangeState(ZombieState.Comeback);
            return;
        }

        // Attack when distance self and player is less than attack range
        if (Vector3.Distance(Zombie.transform.position, Zombie.Player.transform.position) <= Zombie.AttackRange)
        {
            Zombie.Animator.Play("AttackDelay");
            Zombie.ChangeState(ZombieState.Attack);
            return;
        }

        _destinationTimer += Time.deltaTime;
        if (_destinationTimer >= DestinationUpdateInterval)
        {
            _destinationTimer = 0f;
            Zombie.Agent.SetDestination(Zombie.Player.transform.position);
        }
    }
}