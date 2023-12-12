using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIdleState : CreatureBase
{
    public CreatureIdleState(CreatureController sc) : base(sc)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        creature.Animator.SetBool("IsMoving", false);
    }
    public override void OnExit()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (CheckRange())
        {
            if (creature.isAttacking)
                return;
            creatureController.ChangeState(StateController.State.Attack);
            return;
        }
        else
        {
            creatureController.ChangeState(StateController.State.Move);
            return;
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
