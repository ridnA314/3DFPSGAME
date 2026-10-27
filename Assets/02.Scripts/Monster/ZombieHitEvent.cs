using UnityEngine;

public class ZombieHitEvent : MonoBehaviour
{
    private Zombie _zombie;

    private void Start()
    {
        _zombie = GetComponentInParent<Zombie>();
    }

    public void HitEvent()
    {
        if (_zombie.States.TryGetValue(ZombieState.Attack, out ZombieStateBase zombieState))
        {
            ZombieAttackState state = (ZombieAttackState)zombieState;
            state.AttackAction();
        }
    }
}