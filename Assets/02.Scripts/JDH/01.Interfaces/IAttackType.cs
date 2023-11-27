using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackType
{
    public enum AttackType
    {
        Melee,
        DirectProjectile,
        HowitzerProjectile,
        Count,
    }
    public void Attack();
}
