using System.Collections;
using UnityEngine;

public class CreatureAttackState : CreatureBase
{ 
    public CreatureAttackState(CreatureController cc) : base(cc)
    {
    }
    public override void OnEnter()
    {        
        base.OnEnter();
        creature.PlayAttackAnimation();
        creature.isAttacking = true;
        creatureController.ChangeState(StateController.State.Idle);
        if (creature == null)
            return;
    }
    public override void OnExit()
    {
        base.OnExit();        
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

}
