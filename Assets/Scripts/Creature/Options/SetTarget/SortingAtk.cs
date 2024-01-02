using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingAtk : GetTarget
{
    public override void FilterTarget(ref List<Creature> targets, bool higher = false, int count = 1)
    {        
        targets.Sort((x, y) => x.Status.damage.CompareTo(y.Status.damage));
        if (higher)
            targets.Reverse();
        targets.RemoveRange(count, targets.Count - count);
    }
}
