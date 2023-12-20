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
        //Revival revival = null;
        base.OnEnter();
        //foreach(var buff in creature.activedBuffs)
        //{
        //    revival = buff as Revival;
        //    if (revival != null)
        //        break;
        //}
        //if (revival != null)
        //{
        //    creature.activedBuffs.Remove(revival);
        //    return;
        //}
        creature.Animator.SetTrigger("Dead");        
        creature.isDead = true;
        creature.gameObject.layer = LayerMask.NameToLayer(Layers.Dead);
        creature.HPBars.SetActive(false);
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
