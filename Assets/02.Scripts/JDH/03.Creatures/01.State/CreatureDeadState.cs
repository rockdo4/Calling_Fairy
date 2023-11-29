using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDeadState : CreatureBase
{
    public CreatureDeadState(CreatureController creatureController) : base(creatureController)
    {
    }
    public override void OnEnter()
    {
        Revival revival = null;
        base.OnEnter();
        foreach(var buff in creature.buffs)
        {
            revival = buff as Revival;
            if (revival != null)
                break;
        }
        if (revival != null)
        {
            creature.buffs.Remove(revival);
            return;
        }
        
        creature.isDead = true;
        // ���� ���� �׷��� �ٲٱ�
    }
    public override void OnExit()
    {
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnFixedUpdate()
    {
    }
}
