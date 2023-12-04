using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public class Fairy : Creature
{
    protected override void Start()
    {
        stageManager.playerParty.Add(this);
        base.Start();
    }
    public void SetData(FairyCard fairyCard)
    {
        isLoaded = true;
        var table = DataTableMgr.GetTable<CharacterTable>();
        var stat = table.dic[fairyCard.ID];
        realStatus.hp = stat.CharMaxHP + (stat.CharHPIncrease * fairyCard.Level);
        realStatus.physicalAttack = stat.CharPAttack + (stat.CharPAttackIncrease * fairyCard.Level);
        realStatus.magicalAttack = stat.CharMAttack + (stat.CharMAttackIncrease * fairyCard.Level);
        realStatus.physicalArmor = stat.CharPDefence + (stat.CharPDefenceIncrease * fairyCard.Level);
        realStatus.magicalArmor = stat.CharMDefence + (stat.CharMDefenceIncrease * fairyCard.Level);
        realStatus.criticalChance = stat.CharCritRate;
        realStatus.criticalFactor = stat.CharCritFactor;
        realStatus.evasion = stat.CharAvoid;
        realStatus.accuracy = stat.CharAccuracy;
        realStatus.attackSpeed = stat.CharSpeed;
        realStatus.attackRange = stat.CharAttackRange;
        realStatus.basicMoveSpeed = stat.CharMoveSpeed;
        realStatus.moveSpeed = 5f;
        realStatus.knockbackDistance = stat.CharKnockback;
        realStatus.knockbackResist = stat.CharResistance;
        realStatus.attackFactor = stat.CharAttackFactor;
        realStatus.projectileDuration = stat.CharAttackProjectile;
        realStatus.projectileHeight = stat.CharAttackHeight;
        attackType = stat.CharAttackType switch
        {
            1 => IAttackType.AttackType.Melee,
            2 => IAttackType.AttackType.Projectile,
            _ => IAttackType.AttackType.Count,
        };
        targettingType = GetTarget.TargettingType.AllInRange;
        returnStatus = realStatus;


        foreach (var testSkill in TestSkills)
        {
            var skill = SkillBase.MakeSkill(testSkill, this);
            skills.Push(skill);
            switch (testSkill.ID % 100)
            {
                case 1:
                    NormalSkill += skill.Active;
                    break;
                case 2:
                    ReinforcedSkill += skill.Active;
                    break;
                case 3:
                    SpecialSkill += skill.Active;
                    break;
                default:
                    break;
            }
        }
        curHP = Status.hp;
    }
}
