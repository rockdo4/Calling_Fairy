using UnityEngine;

[CreateAssetMenu(fileName = "Basic Status.Asset", menuName = "Status/Basic Status")]
public class BasicStatus : ScriptableObject
{
    public float hP;
    public float physicalAttack;
    public float magicalAttack;
    public float physicalArmor;
    public float magicalArmor;
    public float evasion;
    public float accuracy;
    public float AttackSpeed;
    public bool isMeleeAttack;
}
