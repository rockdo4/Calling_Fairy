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

}
