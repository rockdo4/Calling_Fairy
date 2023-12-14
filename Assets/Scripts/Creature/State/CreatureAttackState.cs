using System.Collections;
using UnityEngine;

public class CreatureAttackState : CreatureBase
{ 
    public CreatureAttackState(CreatureController cc) : base(cc)
    {
    }
    public override void OnEnter()
    {
        //Debug.Log($"{creature.gameObject.name},{creature.attackType}  enterAttack");
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
        if(creature.attackType == AttackType.Projectile)
            Debug.Log($"{creature.gameObject.name},{creature.attackType}  exitAttack");
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
