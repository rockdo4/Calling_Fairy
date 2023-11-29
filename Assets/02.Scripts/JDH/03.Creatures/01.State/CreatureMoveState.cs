using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureMoveState : CreatureBase
{
    public CreatureMoveState(CreatureController sc) : base(sc)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var moveAmount = new Vector2(creature.basicStatus.basicMoveSpeed * Time.deltaTime, 0);
        moveAmount *= creature.basicStatus.moveSpeed;
        creature.Rigidbody.position += moveAmount;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();  
        if(CheckRange())
        {
            if (creature.isAttacking)
            {
                creatureController.ChangeState(StateController.State.Idle);
                return;
            }
            else
            {
                creatureController.ChangeState(StateController.State.Attack);
                return;
            }
        }
    }
}
