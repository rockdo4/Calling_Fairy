using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingAtk : MonoBehaviour, IGetTarget
{
    Creature creature;
    private void Awake()
    {
        creature = GetComponent<Creature>();
    }
    public void GetTarget(float range)
    {
        List<float> comp = new();
        foreach (var target in creature.targets)
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
        Array.Sort(creature.targets.ToArray(), comp.ToArray());
    }
}
