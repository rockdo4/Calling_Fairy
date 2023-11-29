using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
}

public class CreatureBase : BaseState
{
    public CreatureController creatureController;
    protected Creature creature;
    protected float timer;

    public CreatureBase(CreatureController creatureController)
    {
        this.creatureController = creatureController;
        creature = creatureController.creature;
    }
    public override void OnEnter()
    {
        timer = 0f;
    }
    public override void OnExit()
    {
    }
    public override void OnUpdate()
    {
        timer += Time.deltaTime;
    }
    public override void OnFixedUpdate()
    {
    }

    public bool CheckRange()
    {
        creature.targets.Clear();
        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, creature.basicStatus.AttackRange);
        foreach (var target in allTargets)
        {
            var targetCreature = target.GetComponent<IDamagable>();
            if (targetCreature == null || target.gameObject.layer == creature.gameObject.layer)
                continue;
            var targetScript = target.GetComponent<Creature>();
            if(!targetScript.isDead)
            {
                creature.targets.Add(targetScript);
            }
        }
        return creature.targets.Count != 0;
    }

}
