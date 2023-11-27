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
        creature.targets = null;
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var moveAmount = new Vector2(creature.basicStatus.moveSpeed * Time.deltaTime, 0);
        creature.Rigidbody.position += moveAmount;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        bool canAttack = false;
        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, creature.basicStatus.AttackRange);
        foreach (var target in allTargets)
        {
            var targetCreature = target.GetComponent<IDamagable>();
            if (targetCreature == null || target.gameObject.layer == creature.gameObject.layer)
                continue;
            canAttack = true;
            break;
        }        
        if(canAttack)
        {
            if (creature.isAttacked)
                return;
            creatureController.ChangeState(StateController.State.Attack);
            return;
        }
    }
}
