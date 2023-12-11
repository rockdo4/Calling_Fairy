using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BuffBase
{
    public float leftshield;
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        leftshield = buffInfo.value;
        creature.Shields.AddFirst(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(leftshield <= 0)
        {
            creature.RemoveBuff(this);
        }
    }

    public float DamagedShield(float damage)
    {
        if (leftshield > damage)
        {
            leftshield -= damage;
            return 0;
        }
        else
        {
            damage -= leftshield;
            leftshield = 0;
            return damage;
        }
    }

    public override void OnExit()
    {        
        creature.Shields.Remove(this);
    }
}
