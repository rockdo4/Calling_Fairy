using UnityEngine;

public enum CardTypes
{
    All = -1,
    Tanker,
    Dealer,
    Strategist, //Buffer, Balance
}

public struct ExpData
{
    public int Level { get; set; }
    public int Exp { get; set; }
}

public struct BreakLimitData
{
    public int CharCurrGrade;
    public int CharPieceNeeded;
}

public struct CharData
{
    public int CharID { get; set; }
    public int CharName { get; set; }       //string table id
    public int toolTip { get; set; }        //string table id
    public int CharPosition { get; set; }
    public int CharProperty { get; set; }   //1=사물, 2=식물, 3=동물
    public int CharStartingGrade { get; set; }
    public int damageType { get; set; }     //1=물리, 2=마법, 3=혼합
    public int CharPAttack { get; set; }
    public int CharPAttackIncrease { get; set; }
    public int CharMAttack { get; set; }
    public int CharMAttackIncrease { get; set; }
    public float CharSpeed { get; set; }
    public float CharCritRate { get; set; }
    public int CharMaxHP { get; set; }
    public int CharHPIncrease { get; set; }
    public float CharAccuracy { get; set; }
    public int CharPDefence { get; set; }
    public int CharPDefenceIncrease { get; set; }
    public int CharMDefence { get; set; }
    public int CharMDefenceIncrease { get; set; }
    public float CharAvoid { get; set; }
    public float CharResistance { get; set; }
    public float CharAttackFactor { get; set; }
    public int CharAttackType { get; set; }     //1=근거리, 2=원거리
    public int CharAttackRange { get; set; }
    public float CharAttackProjectile { get; set; }
    public int CharAttackHeight { get; set; }
    public int CharMoveSpeed { get; set; }
    public int CharSkill { get; set; }
    public int CharPiece { get; set; }
    public string CharAsset { get; set; }

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
    public float physicalAttack;
    public float magicalAttack;
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
            physicalAttack = 1f;
            magicalAttack = 1f;
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
            physicalAttack = 0f;
            magicalAttack = 0f;
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
        rtn.physicalAttack = lhs.physicalAttack + rhs.physicalAttack;
        rtn.magicalAttack = lhs.magicalAttack + rhs.magicalAttack;
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
        rtn.physicalAttack = lhs.physicalAttack * rhs.physicalAttack;
        rtn.magicalAttack = lhs.magicalAttack * rhs.magicalAttack;
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
    public string SupportName { get; set; }
    public int SupportStartingGrade { get; set; }
    public float SupportAttack { get; set; }
    public float SupportAtkIncrease { get; set; }
    public float SupportMaxHP { get; set; }
    public float SupportHPIncrease { get; set; }
    public int SupportPiece { get; set; }
    public int SupportPieceID { get; set; }
}