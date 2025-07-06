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
        creature.animator.SetBool(Triggers.IsMoving, false);
    }
    public override void OnExit()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!CheckRange())
        {
            creatureController.ChangeState(StateController.State.Move);
            return;
        }

        if (creature.isAttacking)
            return;

        creatureController.ChangeState(StateController.State.Attack);
    }
}
