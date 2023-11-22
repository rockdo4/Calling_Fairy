using UnityEngine;

public class Monster : Creature
{
    protected override void Awake()
    {
        base.Awake();
        inGameManager.monsterParty.AddFirst(gameObject);
    }
}
