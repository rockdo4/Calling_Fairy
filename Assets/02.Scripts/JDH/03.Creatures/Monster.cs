using UnityEngine;

public class Monster : Creature
{
    protected override void Awake()
    {
        base.Awake();
        stageManager.monsterParty.AddFirst(gameObject);
    }
}
