using UnityEngine;

[CreateAssetMenu(fileName = "Basic Status.Asset", menuName = "Status/Basic Status")]
public class SOBasicStatus : ScriptableObject
{
    public float hP;
    public float physicalAttack;
    public float magicalAttack;
    public float physicalArmor;
    public float magicalArmor;
    public float evasion;
    public float accuracy;
    public float AttackSpeed;
    public float AttackRange;
    public float basicMoveSpeed = 1.5f;
    public float moveSpeed = 1.5f;
    public float KnockbackDistance = 100f;
    public float knockbackResist = 0.1f;
    public float attackFactor = 1f;
    public float projectileDuration = 3f;
    public float projectileHeight = 3f;
    public IAttackType.AttackType attackType;
    public GetTarget.TargettingType targettingType;
}
