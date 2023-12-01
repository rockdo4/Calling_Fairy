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
        if (creature == null)
            return;
        creature.Attack();  
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        creatureController.ChangeState(StateController.State.Idle);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

}
