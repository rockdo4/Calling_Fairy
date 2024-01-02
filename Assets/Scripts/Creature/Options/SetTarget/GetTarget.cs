using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GetTarget : MonoBehaviour
{
    public enum TargettingType
    {
        AllInRange,
        SortingDistance,
        SortingHp,
        SortingAtk,
        Count,
    }
    public abstract void FilterTarget(ref List<Creature> target, bool higher = false, int count = 1);
}
