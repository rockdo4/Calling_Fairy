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
        isLoaded = true;
        var table = DataTableMgr.GetTable<MonsterTable>();
        var stat = table.dic[MonsterId];
        realStatus.hp = stat.monMaxHP;
        realStatus.physicalAttack = stat.monPAttack;
        realStatus.magicalAttack = stat.monMAttack;
        realStatus.physicalArmor = stat.monPDefence;
        realStatus.magicalArmor = stat.monMDefence;
        realStatus.criticalChance = stat.monCritRate;
        realStatus.criticalFactor = stat.monCriFactor;
        realStatus.evasion = stat.monAvoid;
        realStatus.accuracy = stat.monAccuracy;
        realStatus.attackSpeed = stat.monSpeed;
        realStatus.attackRange = stat.monAttackRange;
        realStatus.basicMoveSpeed = stat.monMoveSpeed;
        realStatus.moveSpeed = 5f;
        realStatus.knockbackDistance = stat.monKnockback;
        realStatus.knockbackResist = stat.monResistance;
        realStatus.attackFactor = stat.monAttackFactor;
        realStatus.projectileDuration = stat.monAttackProjectile;
        realStatus.projectileHeight = stat.monAttackHeight;
        attackType = stat.monAttackType switch
        {
            1 => IAttackType.AttackType.Melee,
            2 => IAttackType.AttackType.Projectile,
            _ => IAttackType.AttackType.Count,
        };
        targettingType = GetTarget.TargettingType.AllInRange;
        returnStatus = realStatus;
        curHP = Status.hp;
    }
}
