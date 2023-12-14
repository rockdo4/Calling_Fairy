using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackType
{
    private AttackInfo attack;
    private Creature creature;

    private void Awake()
    {
        creature = GetComponent<Creature>();

        attack.attacker = creature.gameObject;
        attack.accuracy = creature.Status.accuracy;
        attack.knockbackDistance = creature.Status.knockbackDistance;
        attack.damage = creature.Status.damage;
        attack.damageType = creature.Status.damageType;
        attack.attackType = AttackType.Melee;
    }
    public void Attack()
    {        
        foreach(var target in creature.targets)
        {
            if( Random.value < creature.Status.criticalChance )
            {
                var criticalAttack = attack;
                criticalAttack.damage *= creature.Status.criticalFactor;
                criticalAttack.isCritical = true;
                target?.GetComponent<IDamagable>().OnDamaged(criticalAttack);
            }
            else
            {
                target?.GetComponent<IDamagable>().OnDamaged(attack);
            }
        }
    }
}
