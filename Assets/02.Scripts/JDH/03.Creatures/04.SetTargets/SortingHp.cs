using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SortingHp : MonoBehaviour, IGetTarget
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
            comp.Add(target.curHP);
        }
        Array.Sort(creature.targets.ToArray(), comp.ToArray());
    }
}
