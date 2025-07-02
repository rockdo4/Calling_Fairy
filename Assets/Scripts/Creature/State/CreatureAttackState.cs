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
    }
}
