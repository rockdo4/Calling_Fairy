using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingAtk : GetTarget
{
    public override void FilterTarget(ref List<Creature> targets)
    {
        List<float> comp = new();
        foreach (var target in targets)
        {
            if (target.basicStatus.physicalAttack == 0)
            {
                comp.Add(target.basicStatus.magicalAttack);
            }
            else
            {
                comp.Add(target.basicStatus.physicalAttack);
            }
        }
        Array.Sort(targets.ToArray(), comp.ToArray());
    }
}
