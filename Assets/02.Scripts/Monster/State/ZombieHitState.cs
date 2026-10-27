using UnityEngine;

// attacked and then stun just moment
public class ZombieHitState : ZombieStateBase
{
    private float _stunTimer;

    public ZombieHitState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        _stunTimer = 0f;
        Zombie.StopAgentMove();
    }

    public override void Tick()
    {
        _stunTimer += Time.deltaTime;
        if (_stunTimer >= Zombie.StunTime)
        {
            Zombie.Animator.Play("Run");
            // Trace because attacked
            Zombie.ChangeState(ZombieState.Trace);
        }
    }
}