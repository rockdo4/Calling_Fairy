using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Info.Asset", menuName = "Status/Skill Info")]
public class SOSkillInfo : ScriptableObject
{
    public int ID;
    public IDamaged.DamageType damageType;
    public IAttackType.AttackType AttackType;
    public float factor; 
    public IngameStatus.StatusType statusType;
    public GetTarget.TargettingType targettingType;
    public SkillTarget skillTarget;
    public BuffInfo buffInfo;
    public GameObject projectile;
    public float airborneDistance;
    public float knockbackDistance;
    public float projectileDuration;
    public float projectileHeight;
    public int relatedCharacterId;
    public float range;
}
