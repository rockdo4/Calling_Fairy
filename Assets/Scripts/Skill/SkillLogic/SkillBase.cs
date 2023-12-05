using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static IAttackType;

public class SkillBase
{
    protected int ID;
    protected Creature creature;
    protected GetTarget getTarget;
    protected AttackInfo attackInfo;
    protected List<Creature> targets = new();

    public static SkillBase MakeSkill(int ID, Creature creature)
    {
        SkillBase rtn;
        var skillData = DataTableMgr.GetTable<SkillTable>().dic[ID];
        if()
        {

        }
        return rtn;
    }
    public virtual void SetData(int ID, Creature creature)
    {
        this.skillInfo = skillInfo;
        this.creature = creature;
        attackInfo.damageType = skillInfo.damageType;
        attackInfo.attacker = this.creature.gameObject;
        attackInfo.buffInfo = skillInfo.buffInfo;
        attackInfo.airborneDistance = skillInfo.airborneDistance;
        attackInfo.knockbackDistance = skillInfo.knockbackDistance;
        attackInfo.accuracy = float.MaxValue;
        //GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();        
        getTarget = skillInfo.targettingType switch
        {
            GetTarget.TargettingType.AllInRange => new AllInRange(),
            GetTarget.TargettingType.SortingAtk => new SortingAtk(),
            GetTarget.TargettingType.SortingHp => new SortingHp(),
            _ => null
        };
    }
    public virtual void Active()
    {
        var dmg = skillInfo.statusType switch
        {
            IngameStatus.StatusType.Hp => creature.Status.hp,
            IngameStatus.StatusType.CurHp => creature.curHP,
            IngameStatus.StatusType.PhysicalAttack => creature.Status.physicalAttack,
            IngameStatus.StatusType.MagicalAttack => creature.Status.magicalAttack,
            IngameStatus.StatusType.PhysicalArmor => creature.Status.physicalArmor,
            IngameStatus.StatusType.MagicalArmor => creature.Status.magicalArmor,
            IngameStatus.StatusType.CriticalChance => creature.Status.criticalChance,
            IngameStatus.StatusType.CriticalFactor => creature.Status.criticalFactor,
            IngameStatus.StatusType.Evasion => creature.Status.evasion,
            IngameStatus.StatusType.Accuracy => creature.Status.accuracy,
            IngameStatus.StatusType.AttackSpeed => creature.Status.attackSpeed,
            IngameStatus.StatusType.AttackRange => creature.Status.attackRange,
            IngameStatus.StatusType.BasicMoveSpeed => creature.Status.basicMoveSpeed,
            IngameStatus.StatusType.MoveSpeed => creature.Status.moveSpeed,
            IngameStatus.StatusType.KnockbackDistance => creature.Status.knockbackDistance,
            IngameStatus.StatusType.KnockbackResist => creature.Status.knockbackResist,
            IngameStatus.StatusType.AttackFactor => creature.Status.attackFactor,
            IngameStatus.StatusType.ProjectileDuration => creature.Status.projectileDuration,
            IngameStatus.StatusType.ProjectileHeight => creature.Status.projectileHeight,
            _ => 0f
        };;

        dmg *= skillInfo.factor;
        attackInfo.damage = dmg;
    }

    protected void GetTargets()
    {
        targets.Clear();
        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, skillInfo.range);
        foreach(var targetcol in allTargets)
        {
            var targetScript = targetcol.GetComponent<Creature>();
            if (targetScript == null)
                continue;
            switch (skillInfo.skillTarget)
            {
                case SkillTarget.Self:
                    if(targetcol == creature)
                    {
                        targets.Add(targetScript);
                    }
                    break;
                case SkillTarget.Ally:
                    if(targetcol.gameObject.layer ==creature.gameObject.layer && targetcol != creature)
                    {
                        targets.Add(targetScript);
                    }
                    break;
                case SkillTarget.Enemy:
                    if(targetcol.gameObject.layer != creature.gameObject.layer)
                    {
                        targets.Add(targetScript);
                    }
                    break;
            }
        }
    }
}

public enum SkillTarget
{
    Self,
    Ally,
    Enemy,
}