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
    }
    public override void OnExit()
    {
        base.OnEnter();
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
            if (curdistance < distance)
            {
                creature.target = targetCreature;
                distance = curdistance;
            }
        }
        if (creature.target != null)
        {
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
