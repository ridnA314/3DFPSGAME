public abstract class ZombieStateBase
{
    protected readonly Zombie Zombie;

    protected ZombieStateBase(Zombie zombie)
    {
        Zombie = zombie;
    }

    public virtual void Enter() { }

    public abstract void Tick();

    public virtual void Exit() { }
}
