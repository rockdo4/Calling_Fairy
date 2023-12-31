using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingDistance : GetTarget
{
    public override void FilterTarget(ref List<Creature> targets, bool higher = false, int count = 1)
    {
        targets.Sort((x, y) => Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position)));
        if (higher)
            targets.Reverse();
        targets.RemoveRange(count, targets.Count - count);
    }
}