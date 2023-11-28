using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public class Damaged : MonoBehaviour, IDamaged
{
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        Debug.Log($"{gameObject.name}이 피격 되엇씁ㄴ다");
        var creatureInfo = deffender.GetComponent<Creature>();
        var calculatedDamage = attack.damage - attack.damageType switch
        {
            DamageType.Magical => creatureInfo.basicStatus.magicalArmor,
            DamageType.Physical => creatureInfo.basicStatus.physicalArmor,
            _=> 0f
        };
        if(Random.value > attack.accuracy - creatureInfo.basicStatus.evasion)
        creatureInfo.curHP -= calculatedDamage;

        if(creatureInfo.curHP <= 0f)
        {
            creatureInfo.OnDestructed();
        }
    }
}
