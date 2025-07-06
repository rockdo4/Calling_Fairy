using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetTarget
{
    public enum TargettingType
    {
        AllInRange,
        SortingDistance,
        SortingHp,
        SortingAtk,
        Count,
    }
    public void FilterTarget(ref List<Creature> target, bool higher = false, int count = 1);
}
