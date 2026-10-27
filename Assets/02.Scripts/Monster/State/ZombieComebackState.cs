// return to initial position

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
        if (Zombie.IsNear(Zombie.StartPosition))
        {
            Zombie.ChangeState(ZombieState.Idle);
        }
    }
}