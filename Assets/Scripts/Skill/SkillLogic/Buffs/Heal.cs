using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BuffBase
{
    protected float healAmount;
    private float tickRate;
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
        tickRate += Time.deltaTime;
        if (tickRate < 1)
        {
            return;
        }
        tickRate = 0;
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