using UnityEngine.AI;

// return to initial position
public class ZombieComebackState : ZombieStateBase
{
    public ZombieComebackState(Zombie zombie) : base(zombie) { }

    public override void Tick()
    {
        // When achieve at first position, trans to Idle State
        if (Zombie.Agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Zombie.ChangeState(ZombieState.Idle);
            return;
        }

        Zombie.Agent.SetDestination(Zombie.StartPosition);
    }
}
