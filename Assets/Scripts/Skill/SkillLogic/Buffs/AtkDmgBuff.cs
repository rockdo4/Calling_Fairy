using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkDmgBuff : BuffBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
        if(buffInfo.isPercent)
        {
            var changeValue = new IngameStatus()
            {
                damage = buffInfo.value / 100f
            };
            creature.MultipleStatus *= changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                damage = buffInfo.value
            };
            creature.PlusStatus += changeValue;
        }
        Debug.Log("AtkDmgBuff OnEnter");
    }

    public override void OnExit()
    {
        Debug.Log("AtkDmgBuff OnExit");
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus()
            {
                damage = -buffInfo.value / 100f
            };
            creature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus()
            {
                damage = -buffInfo.value
            };
            creature.PlusStatus += changeValue;
        }
    
    }

    public override void OnUpdate()
    {        
        base.OnUpdate();
    }
}
