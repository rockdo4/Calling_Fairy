using UnityEngine;

public class Damaged : MonoBehaviour, IDamaged
{
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        var creatureInfo = deffender.GetComponent<Creature>();
        if (creatureInfo.isDead)
            return;
        var calculatedDamage = attack.damage /(1 + (attack.damageType switch
        {
            DamageType.Magical => creatureInfo.Status.magicalArmor,
            DamageType.Physical => creatureInfo.Status.physicalArmor,
            _=> 0f
        } / 100));
        if(calculatedDamage < 0)
        {
            calculatedDamage = 0f;
        }
        creatureInfo.damageIndicator.IndicateDamage(attack.damageType, calculatedDamage, attack.isCritical, false);
        creatureInfo.Damaged(calculatedDamage);
        if(creatureInfo.curHP <= 0)
        {
            creatureInfo.OnDestructed();
        }        
    }
}
