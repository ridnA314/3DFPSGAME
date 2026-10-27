using UnityEngine;

// lay on ground, and do not act
public class ZombieDeathState : ZombieStateBase
{
    private float _destroyDelay;

    private float _deathTimer;

    public ZombieDeathState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        _deathTimer = 0f;
        _destroyDelay = Zombie.DeathWaitingTime;
        Zombie.StopAgentMove();
    }

    public override void Tick()
    {
        _deathTimer += Time.deltaTime;
        if (_deathTimer >= _destroyDelay)
        {
            //Object.Destroy(Zombie.gameObject);
            ZombiePoolManager.Instance?.Release(Zombie);
        }
    }
}