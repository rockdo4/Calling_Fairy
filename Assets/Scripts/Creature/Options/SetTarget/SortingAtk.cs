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
            comp.Add(target.Status.damage);
        }
        Array.Sort(targets.ToArray(), comp.ToArray());
    }
}
