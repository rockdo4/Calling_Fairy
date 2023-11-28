using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackType
{
    public enum AttackType
    {
        Melee,
        Projectile,
        Count,
    }
    public void Attack();
}
