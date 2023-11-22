using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Creature
{
    protected override void Awake()
    {
        base.Awake();
        stageManager.playerParty.Add(gameObject);
    }
}
