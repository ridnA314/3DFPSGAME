// patrol to certain position

using UnityEngine.AI;

public class ZombiePatrolState : ZombieStateBase
{
    public ZombiePatrolState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        //ToDo. Play Walk animation
        // destination is fixed, so request it once (re-requesting every frame keeps the path pending)
        Zombie.Agent.SetDestination(Zombie.PatrolTargetPosition);
    }

    public override void Tick()
    {
        //if (Zombie.PlanarDistanceTo(Zombie.PatrolTargetPosition) < Zombie.ArrivalTolerance ||
        //    Zombie.HasReachedDestination())
        if (Zombie.Agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Zombie.ChangeState(ZombieState.Idle);
        }
    }
}