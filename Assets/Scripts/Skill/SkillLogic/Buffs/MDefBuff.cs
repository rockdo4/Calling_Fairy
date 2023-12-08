using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDefBuff : BuffBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus()
            {
                magicalArmor = buffInfo.value / 100f
            };
            creature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                magicalArmor = buffInfo.value
            };
            creature.PlusStatus += changeValue;
        }
    }

    public override void OnExit()
    {
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus()
            {
                magicalArmor = -buffInfo.value / 100f
            };
            creature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                magicalArmor = -buffInfo.value
            };
            creature.PlusStatus += changeValue;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
