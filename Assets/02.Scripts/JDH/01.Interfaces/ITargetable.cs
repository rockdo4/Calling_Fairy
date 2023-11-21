using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamagable;

public interface ITargetable
{
    public void OnTargeted(float Damage, DamageType damageType);
}
