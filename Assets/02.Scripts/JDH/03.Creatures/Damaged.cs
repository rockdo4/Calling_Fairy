using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public class Damaged : MonoBehaviour, IDamaged
{
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        var creatureInfo = deffender.GetComponent<Creature>();
        var calculatedDamage = attack.damage - attack.damageType switch
        {
            DamageType.Magical => creatureInfo.basicStatus.magicalArmor,
            DamageType.Physical => creatureInfo.basicStatus.physicalArmor,
            _=> 0f
        };
        creatureInfo.curHP -= calculatedDamage;

        if(creatureInfo.curHP <= 0f)
        {
            creatureInfo.OnDestructed();
        }
    }
}
