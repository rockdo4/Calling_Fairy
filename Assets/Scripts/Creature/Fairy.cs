
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
        gameObject.tag = Tags.Player;
        gameObject.layer = LayerMask.NameToLayer(Layers.Player);
        isLoaded = true;
        var table = DataTableMgr.GetTable<CharacterTable>();
        var stat = table.dic[fairyCard.ID];
        var fairyStat = fairyCard.FinalStat;
        realStatus.hp = fairyStat.hp;
        realStatus.damage = fairyStat.attack;
        realStatus.damageType = DamageType.Physical;
        realStatus.physicalArmor = fairyStat.pDefence;
        realStatus.magicalArmor = fairyStat.mDefence;
        realStatus.criticalChance = fairyStat.criticalRate;
        realStatus.criticalFactor = stat.CharCritFactor;
        realStatus.evasion = fairyStat.avoid;
        realStatus.accuracy = fairyStat.accuracy;
        realStatus.attackSpeed = fairyStat.attackSpeed;
        realStatus.attackRange = stat.CharAttackRange;
        realStatus.basicMoveSpeed = stat.CharMoveSpeed;
        realStatus.moveSpeed = 100f;
        realStatus.knockbackDistance = stat.CharKnockback;
        realStatus.knockbackResist = fairyStat.resistance;
        realStatus.attackFactor = stat.CharAttackFactor;
        realStatus.projectileDuration = stat.CharAttackProjectile;
        realStatus.projectileHeight = stat.CharAttackHeight;
        attackType = stat.CharAttackType switch
        {
            1 => AttackType.Melee,
            2 => AttackType.Projectile,
            _ => AttackType.Count,
        };
        targettingType = GetTarget.TargettingType.AllInRange;
        returnStatus = realStatus;
                
        var skillTable = DataTableMgr.GetTable<SkillTable>();
        var normalSkillData = skillTable.dic[stat.CharSkill1];
        var normalSkill = SkillBase.MakeSkill(normalSkillData, this);
        skills.Push(normalSkill);
        NormalSkill += normalSkill.Active;
        //normalSkillCastTime = normalSkillData.skill_atkframe;


        var reinforceSkillData = skillTable.dic[stat.CharSkill2];
        var reinforceSkill = SkillBase.MakeSkill(reinforceSkillData, this);
        skills.Push(reinforceSkill);
        ReinforcedSkill += reinforceSkill.Active;
        //reinforceSkillCastTime = reinforceSkillData.skill_atkframe;
    }
    public override void Damaged(float amount)
    {
        base.Damaged(amount);
        //HPUi에서 업데이트하는 함수 호출해야됨
    }
}
