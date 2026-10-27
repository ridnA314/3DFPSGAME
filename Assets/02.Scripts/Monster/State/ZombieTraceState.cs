using UnityEngine;

// chase to player
public class ZombieTraceState : ZombieStateBase
{
    public ZombieTraceState(Zombie zombie) : base(zombie)
    {
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

        Zombie.Agent.SetDestination(Zombie.Player.transform.position);
    }
}