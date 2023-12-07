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
        if (creature.targettingType == GetTarget.TargettingType.AllInRange)
        {
            foreach(var target in creature.targets)
            {
                target.GetComponent<IDamagable>().OnDamaged(attack);
            }
        }
        else
        {
            if(highValue)
            {
                creature.targets[^1].GetComponent<IDamagable>().OnDamaged(attack);
            }
            else
            {
                creature.targets[0].GetComponent<IDamagable>().OnDamaged(attack);
            }
        }
    }
}
