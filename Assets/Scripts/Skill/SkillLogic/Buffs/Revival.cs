using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival : BuffBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        buffInfo.isDebuff = false;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
