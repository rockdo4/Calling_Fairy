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