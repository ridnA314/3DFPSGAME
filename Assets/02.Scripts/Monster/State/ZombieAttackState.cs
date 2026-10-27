using UnityEngine;

// attack per certain interval
public class ZombieAttackState : ZombieStateBase
{
    private const float AttackInterval = 10f;

    private float _attackTimer;
    private Player _targetPlayer;

    public ZombieAttackState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        _attackTimer = 0f;
        _targetPlayer = Zombie.Player.GetComponent<Player>();
    }

    public override void Tick()
    {
        Zombie.StopAgentMove();

        // distance to player greater than attack range
        if (Vector3.Distance(Zombie.transform.position, Zombie.Player.transform.position) > Zombie.AttackRange)
        {
            Zombie.Animator.Play("Run");
            Zombie.ChangeState(ZombieState.Trace);
            return;
        }

        _attackTimer += Time.deltaTime * Zombie.AttackSpeed;
        if (_attackTimer < AttackInterval) return;

        Zombie.Animator.Play("Attack");
        _attackTimer = 0f;
    }

    public void AttackAction()
    {
        Vector3 playerPosition = Zombie.Player.transform.position;
        DamageInfo info = new DamageInfo()
        {
            Value = Zombie.AttackDamage,
            HitPoint = playerPosition,
            HitNormal = (Zombie.transform.position - playerPosition).normalized,
        };

        _targetPlayer?.TakeDamage(info);
    }
}