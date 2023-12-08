using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackType
{
    private AttackInfo attack;
    private Creature creature;
    private bool highValue = false;

    private void Awake()
    {
        creature = GetComponent<Creature>();

        attack.attacker = creature.gameObject;
        attack.accuracy = creature.Status.accuracy;
        attack.knockbackDistance = creature.Status.knockbackDistance;
        attack.damage = creature.Status.damage;
        attack.damageType = creature.Status.damageType;
    }
    public void Attack()
    {        
        foreach(var target in creature.targets)
        {
            target?.GetComponent<IDamagable>().OnDamaged(attack);
        }
    }
}
