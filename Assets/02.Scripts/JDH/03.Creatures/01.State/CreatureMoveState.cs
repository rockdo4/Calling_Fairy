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
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var moveAmount = new Vector2(creature.basicStatus.moveSpeed * Time.fixedDeltaTime, 0);
        creature.Rigidbody.MovePosition((Vector2)creature.transform.position + moveAmount);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, creature.basicStatus.AttackRange);
        float distance = float.MaxValue;
        foreach (var target in allTargets)
        {
            var targetCreature = target.GetComponent<ITargetable>();
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
            creatureController.ChangeState(StateController.State.Attack);
            return;
        }
    }
}
