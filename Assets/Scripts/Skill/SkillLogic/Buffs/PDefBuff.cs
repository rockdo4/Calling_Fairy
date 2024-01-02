using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDefBuff : BuffBase
{    
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus()
            {
                physicalArmor = buffInfo.value / 100f
            };
            creature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                physicalArmor = buffInfo.value
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
                physicalArmor = -buffInfo.value / 100f
            };
            creature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                physicalArmor = -buffInfo.value
            };
            creature.PlusStatus += changeValue;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //Debug.Log("PDefBuff OnUpdate");
    }
}
