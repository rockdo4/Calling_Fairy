using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BuffBase
{
    protected float healAmount;
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        healAmount = buffInfo.value;        
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (timer < 1)
        {
            return;
        }
        timer = 0;
        if (buffInfo.isPercent)
        {
            creature.Heal(creature.Status.hp * healAmount / 100f);
        }
        else
        {
            creature.Heal(healAmount);
        }
    }
}