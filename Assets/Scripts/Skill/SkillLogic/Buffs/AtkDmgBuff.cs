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
            var changeValue = new IngameStatus(IngameStatus.MakeType.Multiple)
            {
                damage = buffInfo.value / 10f
            };
            buffInfo.buffedCreature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Normal)
            {
                damage = buffInfo.value
            };
            buffInfo.buffedCreature.PlusStatus += changeValue;
        }
        Debug.Log("AtkDmgBuff OnEnter");
    }

    public override void OnExit()
    {
        Debug.Log("AtkDmgBuff OnExit");
        if (buffInfo.isPercent)
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Multiple)
            {
                damage = -buffInfo.value / 10f
            };
            buffInfo.buffedCreature.MultipleStatus += changeValue;
        }
        else
        {
            var changeValue = new IngameStatus(IngameStatus.MakeType.Normal)
            {
                damage = -buffInfo.value
            };
            buffInfo.buffedCreature.PlusStatus += changeValue;
        }
    
    }

    public override void OnUpdate()
    {        
        base.OnUpdate();
    }
}
