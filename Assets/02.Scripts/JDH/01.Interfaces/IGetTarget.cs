using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetTarget
{
    public enum TargettingType
    {
        AllInRange,
        LowestHp,
        HighestAtk,
        Count,
    }
    public void GetTarget(float range);
}
