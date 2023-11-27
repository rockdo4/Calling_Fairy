using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllInRange : MonoBehaviour, IGetTarget
{
    Creature creature;
    private void Awake()
    {
        creature = GetComponent<Creature>();
    }
    public void GetTarget(float range)
    {
    }
}
