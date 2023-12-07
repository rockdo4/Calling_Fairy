using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSpeedBuff : BuffBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Multiple)
            {
                attackSpeed = buffInfo.value / 10f
            };
            buffInfo.buffedCreature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Normal)
            {
                attackSpeed = buffInfo.value
            };
            buffInfo.buffedCreature.PlusStatus += changeValue;
        }
    }

    public override void OnExit()
    {
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Multiple)
            {
                attackSpeed = -buffInfo.value / 10f
            };
            buffInfo.buffedCreature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Normal)
            {
                attackSpeed = -buffInfo.value
            };
            buffInfo.buffedCreature.PlusStatus += changeValue;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
