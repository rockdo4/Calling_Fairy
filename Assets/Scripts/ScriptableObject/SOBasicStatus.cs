using UnityEngine;

[CreateAssetMenu(fileName = "Basic Status.Asset", menuName = "Status/Basic Status")]
public class SOBasicStatus : ScriptableObject
{
    public float hp;
    public float physicalAttack;
    public float magicalAttack;
    public float physicalArmor;
    public float magicalArmor;
    public float criticalChance = 0.1f;
    public float criticlaFactor = 1.5f;
    public float evasion;
    public float accuracy;
    public float attackSpeed;
    public float attackRange;
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
