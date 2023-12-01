using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public interface IDamagable
{
    public void OnDamaged(AttackInfo attack);
}
