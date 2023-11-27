using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDeadState : CreatureBase
{
    public CreatureDeadState(CreatureController creatureController) : base(creatureController)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        creature.isDead = true;
        // 죽은 이후 그래픽 바꾸기
    }
    public override void OnExit()
    {
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnFixedUpdate()
    {
    }
}
