using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
        attack.knockbackDistance = creature.Status.KnockbackDistance;
        if (creature.Status.physicalAttack != 0)
        {
            attack.damage = creature.Status.physicalAttack;
            attack.damageType = IDamaged.DamageType.Physical;
        }
        else
        {
            attack.damage = creature.Status.magicalAttack;
            attack.damageType = IDamaged.DamageType.Magical;
        }
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
