using UnityEngine;
public class Monster : Creature
{
    protected override void Awake()
    {
        base.Awake();
        stageManager.monsterParty.AddFirst(gameObject);
        destructableStripts.Add(gameObject.AddComponent<RemoveAtMonsterList>());
    }
    public void SetData(int MonsterId)
    {
        gameObject.tag = Tags.Monster;
        gameObject.layer = LayerMask.NameToLayer(Layers.Monster);
        isLoaded = true;
        var table = DataTableMgr.GetTable<MonsterTable>();
        var stat = table.dic[MonsterId];
        realStatus = new IngameStatus(stat);
        attackType = stat.monAttackType switch
        {
            1 => AttackType.Melee,
            2 => AttackType.Projectile,
            _ => AttackType.Count,
        };
        //targettingType = GetTarget.TargettingType.AllInRange;
        Status = realStatus;
        curHP = Status.hp;
        normalAttackSE = Resources.Load<AudioClip>(stat.AttackSE);
        var di = gameObject.AddComponent<DropItem>();
        di.SetData(stat.dropItem);
        destructableStripts.Add(di);
    }
}
