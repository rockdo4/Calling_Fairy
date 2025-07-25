using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardTypes
{
    All = -1,
    Tanker,
    Dealer,
    Strategist, //Buffer, Balance
}
public struct Stat
{
    public float attack;
    public float pDefence;
    public float mDefence;
    public float hp;
    public float criticalRate;
    public float attackSpeed;
    public float accuracy;
    public float avoid;
    public float resistance;
    public float battlePower;

    public static Stat operator +(Stat lhs, Stat rhs)
    {
        Stat result = new Stat();
        result.attack = lhs.attack + rhs.attack;
        result.pDefence = lhs.pDefence + rhs.pDefence;
        result.mDefence = lhs.mDefence + rhs.mDefence;
        result.hp = lhs.hp + rhs.hp;
        result.criticalRate = lhs.criticalRate + rhs.criticalRate;
        result.attackSpeed = lhs.attackSpeed + rhs.attackSpeed;
        result.accuracy = lhs.accuracy + rhs.accuracy;
        result.avoid = lhs.avoid + rhs.avoid;
        result.resistance = lhs.resistance + rhs.resistance;
        result.battlePower = lhs.battlePower + rhs.battlePower;
        return result;
    }

}

public struct PlayerSaveData
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int MaxExperience { get; set; }
    public int Stamina { get; set; }
    public int MaxStamina { get; set; }
    public DateTime LastRecoveryTime { get; set; }
    public int MainFairyID { get; set; }
    public int SummonStone { get; set; }
    public PlayerSaveData(PlayerTable table)
    {
        Name = "NoName";
        Level = 1;
        Experience = 0;
        MaxExperience = table.dic[Level].PlayerExp;
        MaxStamina = table.dic[Level].PlayerMaxStamina;
        Stamina = MaxStamina;
        LastRecoveryTime = DateTime.Now;
        MainFairyID = 0;
        SummonStone = 0;
    }
}

public struct PlayerData
{
    public int PlayerLevel { get; set; }
    public int PlayerAbility { get; set; }
    public int PlayerMaxStamina { get; set; }
    public int PlayerExp { get; set; }
    public int PlayerTooltip { get; set; }
}

public struct PlayerAbilityData
{
    public int AbilityID { get; set; }
    public int AbilityTooltip { get; set; }
    public float AbilityHP { get; set; }
    public float AbilityDefence { get; set; }
    public float AbilityCriticalRate { get; set; }
    public float AbilityAttack { get; set; }
    public float AbilityStageGold { get; set; }
}


public struct EquipData
{
    public int EquipID { get; set; }
    public int EquipName { get; set; }
    public int EquipType { get; set; }
    public int EquipPosition { get; set; }
    public int EquipSlot { get; set; }
    public int EquipRank { get; set; }
    public float EquipAttack { get; set; }
    public float EquipAttackIncrease { get; set; }
    public float EquipAttackSpeed { get; set; }
    public float EquipCriticalRate { get; set; }
    public int EquipMaxHP { get; set; }
    public int EquipHPIncrease { get; set; }
    public float EquipAccuracy { get; set; }
    public float EquipPDefence { get; set; }
    public float EquipPDefenceIncrease { get; set; }
    public float EquipMDefence { get; set; }
    public float EquipMDefenceIncrease { get; set; }
    public float EquipAvoid {  get; set; }
    public float EquipRegistance { get; set; }
    public int EquipPiece {  get; set; }
    public int EquipPieceNum { get; set; }
    public string EquipIcon { get; set; }
}

public struct ExpData
{
    public int Level { get; set; }
    public int Exp { get; set; }
}

public struct BreakLimitData
{
    public int CharCurrGrade { get; set; }
    public int CharPieceNeeded { get; set; }
}

public struct CharData
{
    //CharID,CharName,toolTip,CharPosition,CharPositionID,CharProperty,CharPropertyID,CharStartingGrade,damageType,CharAttack,CharAttackIncrease,CharSpeed,CharCritRate,CharCritFactor,CharMaxHP,CharHPIncrease,CharAccuracy,CharPDefence,CharPDefenceIncrease,CharMDefence,CharMDefenceIncrease,CharAvoid,CharKnockback,CharResistance,CharAttackFactor,CharAttackType,CharAttackRange,CharAttackProjectile,CharAttackHeight,CharMoveSpeed,CharSkill1,CharSkill2,CharPiece,CharAsset,CharIllust,CharIcon,CharSkillIcon
    public int CharID { get; set; }
    public int CharName { get; set; }       //string table id
    public int toolTip { get; set; }        //string table id
    public int CharPosition { get; set; }
    public int CharPositionID { get; set; }
    public int CharProperty { get; set; }   //1=?�물, 2=?�물, 3=?�물
    public int CharPropertyID { get; set; }
    public int CharStartingGrade { get; set; }
    public int damageType { get; set; }     //1=물리, 2=마법, 3=?�합
    public float CharAttack { get; set; }
    public float CharAttackIncrease { get; set; }
    public float CharSpeed { get; set; }
    public float CharCritRate { get; set; }
    public float CharMaxHP { get; set; }
    public float CharHPIncrease { get; set; }
    public float CharAccuracy { get; set; }
    public float CharPDefence { get; set; }
    public float CharPDefenceIncrease { get; set; }
    public float CharMDefence { get; set; }
    public float CharMDefenceIncrease { get; set; }
    public float CharAvoid { get; set; }
    public float CharResistance { get; set; }
    public float CharAttackFactor { get; set; }
    public int CharAttackType { get; set; }     //1=근거�? 2=?�거�?
    public float CharAttackRange { get; set; }
    public float CharAttackProjectile { get; set; }
    public float CharAttackHeight { get; set; }
    public float CharMoveSpeed { get; set; }
    public int CharSkill1 { get; set; }
    public int CharSkill2 { get; set; }
    public int CharPiece { get; set; }
    public string CharAsset { get; set; }
    public float CharCritFactor { get; set; }
    public float CharKnockback { get; set; }
    public string CharIllust { get; set; }
    public string CharIcon { get; set; }
    public string CharSkillIcon { get; set; }
    public string AttackSE { get; set; }
}

public enum statStatus
{
    Normal=0,
    AttackUp,
    AttackDown,
    AttackNormal,
    PhysicalArmorUp,
    PhysicalArmorDown,
    PhysicalArmorNormal,
    MagicalArmorUp,
    MagicalArmorDown,
    MagicalArmorNormal,
    ASUp,
    ASDown,
    ASNormal,
    Count,
}
public struct IngameStatus
{
    public enum MakeType
    {
        Normal,
        Multiple,
    }
    public enum StatusType
    {
        None,
        Hp,
        CurHp,
        PhysicalAttack,
        MagicalAttack,
        PhysicalArmor,
        MagicalArmor,
        CriticalChance,
        CriticalFactor,
        Evasion,
        Accuracy,
        AttackSpeed,
        AttackRange,
        BasicMoveSpeed,
        MoveSpeed,
        KnockbackDistance,
        KnockbackResist,
        AttackFactor,
        ProjectileDuration,
        ProjectileHeight,
        Option1,
        Option2,
        Option3,
        Count,
    };

    public float hp;
    public float damage;
    public DamageType damageType;
    public float physicalArmor;
    public float magicalArmor;
    public float criticalChance;
    public float criticalFactor;
    public float evasion;
    public float accuracy;
    public float attackSpeed;
    public float attackRange;
    public float basicMoveSpeed;
    public float moveSpeed;
    public float knockbackDistance;
    public float knockbackResist;
    public float attackFactor;
    public float projectileDuration;
    public float projectileHeight;

    public IngameStatus(MakeType make = MakeType.Normal)
    {
        if (make == MakeType.Multiple)
        {
            hp = 1f;
            damage = 1f;
            damageType = DamageType.Physical;
            physicalArmor = 1f;
            magicalArmor = 1f;
            criticalChance = 1f;
            criticalFactor = 1f;
            evasion = 1f;
            accuracy = 1f;
            attackSpeed = 1f;
            attackRange = 1f;
            basicMoveSpeed = 1f;
            moveSpeed = 1f;
            knockbackDistance = 1f;
            knockbackResist = 1f;
            attackFactor = 1f;
            projectileDuration = 1f;
            projectileHeight = 1f;
        }
        else
        {
            hp = 0f;
            damage = 0f;
            damageType = DamageType.Physical;
            physicalArmor = 0f;
            magicalArmor = 0f;
            criticalChance = 0f;
            criticalFactor = 0f;
            evasion = 0f;
            accuracy = 0f;
            attackSpeed = 0f;
            attackRange = 0f;
            basicMoveSpeed = 0f;
            moveSpeed = 0f;
            knockbackDistance = 0f;
            knockbackResist = 0f;
            attackFactor = 0f;
            projectileDuration = 0f;
            projectileHeight = 0f;
        }
    }

    public static IngameStatus operator +(IngameStatus lhs, IngameStatus rhs)
    {
        IngameStatus rtn;
        rtn.hp = lhs.hp + rhs.hp;
        rtn.damage = lhs.damage + rhs.damage;
        rtn.damageType = lhs.damageType;
        rtn.physicalArmor = lhs.physicalArmor + rhs.physicalArmor;
        rtn.magicalArmor = lhs.magicalArmor + rhs.magicalArmor;
        rtn.criticalChance = lhs.criticalChance + rhs.criticalChance;
        rtn.criticalFactor = lhs.criticalFactor + rhs.criticalFactor;
        rtn.evasion = lhs.evasion + rhs.evasion;
        rtn.accuracy = lhs.accuracy + rhs.accuracy;
        rtn.attackSpeed = lhs.attackSpeed + rhs.attackSpeed;
        rtn.attackRange = lhs.attackRange + rhs.attackRange;
        rtn.basicMoveSpeed = lhs.basicMoveSpeed + rhs.basicMoveSpeed;
        rtn.moveSpeed = lhs.moveSpeed + rhs.moveSpeed;
        rtn.knockbackDistance = lhs.knockbackDistance + rhs.knockbackDistance;
        rtn.knockbackResist = lhs.knockbackResist + rhs.knockbackResist;
        rtn.attackFactor = lhs.attackFactor + rhs.attackFactor;
        rtn.projectileDuration = lhs.projectileDuration + rhs.projectileDuration;
        rtn.projectileHeight = lhs.projectileHeight + rhs.projectileHeight;
        return rtn;
    }
    public static IngameStatus operator *(IngameStatus lhs, IngameStatus rhs)
    {
        IngameStatus rtn;
        rtn.hp = lhs.hp * rhs.hp;
        rtn.damage = lhs.damage * rhs.damage;
        rtn.damageType = lhs.damageType;
        rtn.physicalArmor = lhs.physicalArmor * rhs.physicalArmor;
        rtn.magicalArmor = lhs.magicalArmor * rhs.magicalArmor;
        rtn.criticalChance = lhs.criticalChance * rhs.criticalChance;
        rtn.criticalFactor = lhs.criticalFactor * rhs.criticalFactor;
        rtn.evasion = lhs.evasion * rhs.evasion;
        rtn.accuracy = lhs.accuracy * rhs.accuracy;
        rtn.attackSpeed = lhs.attackSpeed * rhs.attackSpeed;
        rtn.attackRange = lhs.attackRange * rhs.attackRange;
        rtn.basicMoveSpeed = lhs.basicMoveSpeed * rhs.basicMoveSpeed;
        rtn.moveSpeed = lhs.moveSpeed * rhs.moveSpeed;
        rtn.knockbackDistance = lhs.knockbackDistance * rhs.knockbackDistance;
        rtn.knockbackResist = lhs.knockbackResist * rhs.knockbackResist;
        rtn.attackFactor = lhs.attackFactor * rhs.attackFactor;
        rtn.projectileDuration = lhs.projectileDuration * rhs.projectileDuration;
        rtn.projectileHeight = lhs.projectileHeight * rhs.projectileHeight;
        return rtn;
    }


}

public struct SupportCardData
{
    public int SupportID { get; set; }
    public int SupportName { get; set; }
    public int SupportStartingGrade { get; set; }
    public float SupportAttack { get; set; }
    public float SupportAtkIncrease { get; set; }
    public float SupportMaxHP { get; set; }
    public float SupportHPIncrease { get; set; }
    public int SupportPiece { get; set; }
    public int SupportPieceID { get; set; }
}

public struct MonsterDropData
{
    public int ID { get; set; }
    //value1 = itemID, value2 = percent
    public Tuple<int, int>[] Drops { get; set; }
}

public struct SkillData
{
    public int skill_ID { get; set; }
    public int skill_group { get; set; }
    public int skill_name { get; set; }
    public int skill_tooltip { get; set; }
    public int skill_kbValue { get; set; }
    public int skill_abValue { get; set; }
    public int skill_motionFollow { get; set; }
    public string skill_animation { get; set; }
    public string skill_icon { get; set; }
    public int skill_projectileID { get; set; }
    public float skill_range { get; set; }
    public string SkillSE { get; set; }
    public List<DetailSkillData> skill_detail { get; set; }
}
public struct DetailSkillData
{
    public TargetingType skill_appType;
    public int skill_practiceType;
    public float skill_multipleValue;
    public int skill_time;
    public int skill_abnormalID;
    public string skill_appAnimation;
    public SkillNumType skill_numType;
}

public struct BoxData
{
    public string box_ID { get; set; }
    public List<DetailBoxData> boxDetailDatas { get; set; }
    //public int boxAllRate { get; set; }
}

public struct DetailBoxData
{
    public int box_itemID { get; set; }
    public int box_Tier { get; set; }
    public int box_itemPercent { get; set; }
}



public struct StageData
{
    public int iD { get; set; }
    public int stagetype { get; set; }
    public int stageName { get; set; }
    public int stageDorpPercent { get; set; }
    public int useStamina { get; set; }
    public int gainPlayerExp { get; set; }
    public int gainExpStone { get; set; }
    public int gainExpStoneValue { get; set; }
    public int gainGold { get; set; }
    public int wave1 { get; set; }
    public int wave2 { get; set; }
    public int wave3 { get; set; }
    public int wave4 { get; set; }
    public int wave5 { get; set; }
    public int wave6 { get; set; }
    public string map { get; set; }
    public float speed1 { get; set; }
    public float speed2 { get; set; }
    public float speed3 { get; set; }
}
public struct MonsterData
{
    public int iD { get; set; }
    public int monsterName { get; set; }
    public int monsterToolTip { get; set; }
    public int monPosition { get; set; }
    public float monPAttack { get; set; }
    public int monAtkPA { get; set; }
    public float monSpeed { get; set; }
    public float monCritRate { get; set; }
    public float monCriFactor { get; set; }
    public float monAttackFactor { get; set; }
    public int monAttackType { get; set; }
    public float monAttackRange { get; set; }
    public float monAttackProjectile { get; set; }
    public float monAttackHeight { get; set; }
    public float monMaxHP { get; set; }
    public float monAccuracy { get; set; }
    public float monPDefence { get; set; }
    public float monMDefence { get; set; }
    public float monAvoid { get; set; }
    public float monKnockback { get; set; }
    public float monResistance { get; set; }
    public float monMoveSpeed { get; set; }
    public float monSkill1 { get; set; }
    public float monSkill2 { get; set; }
    public float monSkill3 { get; set; }
    public float monSkillCooldown { get; set; }
    public int dropItem { get; set; }
    public string asset { get; set; }
    public string monIcon { get; set; }
    public string AttackSE { get; set; }
}

public struct WaveData
{
    public int ID { get; set; }
    public int[] Monsters { get; set; }
    public float spawnTimer { get; set; }
}

public struct ItemData
{
    public int ID { get; set; }
    public int name { get; set; }
    public int sort { get; set; }
    public int value1 { get; set; }
    public int value2 { get; set; }
    public int tooltip { get; set; }
    public string icon { get; set; }
    public string DropStage { get; set; }
}

public struct SkillProjectileData
{
    public int projectile_ID { get; set; }
    public int projectile_name { get; set; }
    public float proj_startOffsetX { get; set; }
    public float proj_startOffsetY { get; set; }
    public float proj_life { get; set; }
    public float proj_highest { get; set; }
    public int proj_follow { get; set; }
    public int proj_speed { get; set; }
    public float proj_acceleration { get; set; }
    public string proj_sprite { get; set; }
}

public struct SkillDebuffData
{
    public int abnormal_ID { get; set; }
    public int abnormal_name { get; set; }
    public float abn_attackStop { get; set; }
    public float abn_skillStop { get; set; }
    public float abn_moveStop { get; set; }
    public int abn_sprite { get; set; }
}

public struct StringData
{
    public int ID { get; set; }
    public string Value
    {
        get
        {
            /*
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    return Korean;
                case SystemLanguage.English:
                    return English;
                default:
                    return Korean;            
            }
            */
            string rtn = StringTable.Lang switch
            {
                StringTable.Language.Korean => Korean,
                StringTable.Language.English => English,
                _ => "Lang Setting Error"
            };
            return rtn.Replace("\\n", "\n");
            //switch (StringTable.Lang)
            //{
            //    case StringTable.Language.Korean:
            //        return Korean.Replace("\\n", "\n");
            //    default:
            //        return "lang setting Error";
            //}
        }
    }
    public string Korean { private get; set; }
    public string English { private get; set; }
}

public enum SkillGroup
{
    Chain2,
    Chain3,
}

public enum AttackType
{
    Melee,
    Projectile,
    Count,
}

public enum TargetingType
{
    Enemy,
    Ally,
    Self,
    Count,
}
public struct AttackInfo
{
    public float damage;
    public DamageType damageType;
    public GameObject attacker;
    public float knockbackDistance;
    public float airborneDistance;
    public float accuracy;
    public BuffInfo buffInfo;
    public TargetingType targetingType;
    public AttackType attackType;
    public bool isCritical;
    public bool isSkill;
    public EffectType effectType;
}

public enum SkillNumType
{
    Int,
    Percent,
}
public enum DamageType
{
    Physical,
    Magical,
}

public enum BuffType
{
    AtkDmgBuff = 2,
    PDefBuff,
    MDefBuff,
    AtkSpdBuff,
    CritRateBuff,
    Heal,
    Shield,
    Revival,
}

public struct ShopData
{
    public int ID { get; set; }
    public int ItemName { get; set; }
    public int Price { get; set; }
    public int Value { get; set; }
    public string Icon { get; set; }
}

public enum IconType
{
    Monster,    
    Item,
    Count,
}

public enum Mode
{
    Story = 1,
    Daily,
}