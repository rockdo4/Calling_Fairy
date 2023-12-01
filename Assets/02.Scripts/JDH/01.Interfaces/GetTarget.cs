using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GetTarget
{
    public enum TargettingType
    {
        AllInRange,
        SortingHp,
        SortingAtk,
        Count,
    }
    public abstract void FilterTarget(ref List<Creature> target);
}
