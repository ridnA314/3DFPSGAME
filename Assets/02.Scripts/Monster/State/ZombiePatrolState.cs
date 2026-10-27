using UnityEngine;

// patrol to certain position
public class ZombiePatrolState : ZombieStateBase
{
    private const float PatrolArrivalDistance = 0.5f;

    public ZombiePatrolState(Zombie zombie) : base(zombie) { }

    public override void Tick()
    {
        if (Vector3.Distance(Zombie.transform.position, Zombie.PatrolTargetPosition) < PatrolArrivalDistance)
        {
            Zombie.ChangeState(ZombieState.Idle);
            return;
        }

        //ToDo. Play Walk animation
        Zombie.Agent.SetDestination(Zombie.PatrolTargetPosition);
    }
}
