using UnityEngine;

// lay on ground, and do not act
public class ZombieDeathState : ZombieStateBase
{
    private const float DestroyDelay = 0.5f;

    private float _deathTimer;

    public ZombieDeathState(Zombie zombie) : base(zombie) { }

    public override void Enter()
    {
        _deathTimer = 0f;
        Zombie.StopAgentMove();
    }

    public override void Tick()
    {
        _deathTimer += Time.deltaTime;
        if (_deathTimer >= DestroyDelay)
        {
            Object.Destroy(Zombie.gameObject);
        }
    }
}
