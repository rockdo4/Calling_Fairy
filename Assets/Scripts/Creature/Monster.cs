using UnityEngine;
public class Monster : Creature
{
    protected override void Awake()
    {
        base.Awake();
        stageManager.monsterParty.AddFirst(gameObject);
        gameObject.AddComponent<RemoveAtMonsterList>();
    }
    public void SetData(int MonsterId)
    {
        gameObject.tag = Tags.Monster;
        gameObject.layer = LayerMask.NameToLayer(Layers.Monster);
        isLoaded = true;
        var table = DataTableMgr.GetTable<MonsterTable>();
        var stat = table.dic[MonsterId];
        realStatus.hp = stat.monMaxHP;
        realStatus.damage = stat.monPAttack;
        realStatus.damageType = DamageType.Physical;
        realStatus.physicalArmor = stat.monPDefence;
        realStatus.magicalArmor = stat.monMDefence;
        realStatus.criticalChance = stat.monCritRate;
        realStatus.criticalFactor = stat.monCriFactor;
        realStatus.evasion = stat.monAvoid;
        realStatus.accuracy = stat.monAccuracy;
        realStatus.attackSpeed = stat.monSpeed;
        realStatus.attackRange = stat.monAttackRange;
        realStatus.basicMoveSpeed = stat.monMoveSpeed;
        realStatus.moveSpeed = -100f;
        realStatus.knockbackDistance = stat.monKnockback;
        realStatus.knockbackResist = stat.monResistance;
        realStatus.attackFactor = stat.monAttackFactor;
        realStatus.projectileDuration = stat.monAttackProjectile;
        realStatus.projectileHeight = stat.monAttackHeight;
        attackType = stat.monAttackType switch
        {
            1 => AttackType.Melee,
            2 => AttackType.Projectile,
            _ => AttackType.Count,
        };
        targettingType = GetTarget.TargettingType.AllInRange;
        returnStatus = realStatus;
        curHP = Status.hp;
        normalAttackSE = Resources.Load<AudioClip>(stat.AttackSE);
        var di = gameObject.AddComponent<DropItem>() ;
        di.SetData(stat.dropItem);
    }
}
