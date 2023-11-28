
using UnityEngine;

public interface IDamaged
{
    public enum DamageType
    {
        Physical,
        Magical,
    }
    public void OnDamage(GameObject deffender, AttackInfo attack);
}

public struct AttackInfo
{
    public float damage;
    public IDamaged.DamageType damageType;
    public GameObject attacker;
    public float knockbackDistance;
    public float airborneDistance;
    public float accuracy;
    public float knockbackResist;
}
