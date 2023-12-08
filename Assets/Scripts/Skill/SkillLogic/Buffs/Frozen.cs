using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : BuffBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = true;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
