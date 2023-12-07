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
        if (buffInfo.isPercent)
        {
            buffInfo.buffedCreature.Heal(buffInfo.buffedCreature.Status.hp * healAmount / 100f);
        }
        else
        {
            buffInfo.buffedCreature.Heal(healAmount);
        }
        base.OnUpdate();
    }
}