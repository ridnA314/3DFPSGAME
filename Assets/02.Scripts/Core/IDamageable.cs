using UnityEngine;

//promise about can taking damage
public interface IDamageable
{
    // the promise means that should implement below method
    public void TakeDamage(DamageInfo damageInfo);
}