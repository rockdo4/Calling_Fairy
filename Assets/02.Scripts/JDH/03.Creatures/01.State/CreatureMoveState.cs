using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CreatureMoveState : CreatureBase
{
    public CreatureMoveState(CreatureController sc) : base(sc)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        creature.target = null;
    }
    public override void OnExit()
    {
        base.OnExit();
        creature.Rigidbody.totalForce = Vector2.zero;
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var moveAmount = new Vector2(creature.basicStatus.moveSpeed, 0);
        creature.Rigidbody.totalForce = moveAmount;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, creature.basicStatus.AttackRange);
        float distance = float.MaxValue;        
        foreach (var target in allTargets)
        {
            var targetCreature = target.GetComponent<IDamagable>();
            if (targetCreature == null || target.gameObject.layer == creature.gameObject.layer)
                continue;
            var curdistance = Vector2.Distance(creature.transform.position, target.transform.position);
            if(curdistance < distance)
            {
                creature.target = targetCreature;
                distance = curdistance;
            }
        }
        if(creature.target != null)
        {
            if (creature.isAttacked)
                return;
            creatureController.ChangeState(StateController.State.Attack);
            return;
        }
    }
}
