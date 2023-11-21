
using UnityEngine;

public interface IDamagable
{
    public enum DamageType
    {
        Physical,
        Magical,
    }
    public void OnDamage(GameObject deffender, float Damage, DamageType damagaType);
}
