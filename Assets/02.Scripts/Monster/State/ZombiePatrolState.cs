// patrol to certain position

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
        if (Zombie.IsNear(Zombie.PatrolTargetPosition))
        {
            Zombie.Animator.SetTrigger("MoveToIdle");
            Zombie.ChangeState(ZombieState.Idle);
        }
    }
}