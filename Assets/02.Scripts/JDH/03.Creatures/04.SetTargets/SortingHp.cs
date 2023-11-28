using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SortingHp : GetTarget
{
    public override void FilterTarget(ref List<Creature> targets)
    {
        List<float> comp = new();
        foreach (var target in targets)
        {            
            comp.Add(target.curHP);
        }
        Array.Sort(targets.ToArray(), comp.ToArray());
    }
}
