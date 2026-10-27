// return to initial position

using UnityEngine.AI;

public class ZombieComebackState : ZombieStateBase
{
    public ZombieComebackState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        Zombie.Agent.SetDestination(Zombie.StartPosition);
    }

    public override void Tick()
    {
        // When achieve at first position, trans to Idle State
        //if (Zombie.PlanarDistanceTo(Zombie.StartPosition) < Zombie.ArrivalTolerance ||
        //    Zombie.HasReachedDestination())
        if (Zombie.Agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Zombie.ChangeState(ZombieState.Idle);
        }
    }
}