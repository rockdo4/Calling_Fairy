
using UnityEngine;

public interface IDamaged
{
    public enum DamageType
    {
        Physical,
        Magical,
    }
    public void OnDamage(GameObject deffender, float Damage, DamageType damagaType);
}
