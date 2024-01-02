using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SortingHp : GetTarget
{
    public override void FilterTarget(ref List<Creature> targets, bool higher = false, int count = 1)
    {
        targets.Sort((x, y) => x.curHP.CompareTo(y.curHP));
        if (higher)
            targets.Reverse();
        targets.RemoveRange(count, targets.Count - count);
    }
}
