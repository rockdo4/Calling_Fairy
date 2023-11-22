using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    protected override void Awake()
    {
        base.Awake();
        inGameManager.playerParty.Add(gameObject);
    }
}
