using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackType
{
    private AttackInfo attack;
    private Creature creature;
    private IGetTarget targetingType;
    private bool highValue = false;

    private void Awake()
    {
        creature = GetComponent<Creature>();

        attack.attacker = creature.gameObject;
        attack.knockbackDistance = creature.basicStatus.KnockbackDistance;
        if (creature.basicStatus.physicalAttack != 0)
        {
            attack.damage = creature.basicStatus.physicalAttack;
            attack.damageType = IDamaged.DamageType.Physical;
        }
        else
        {
            attack.damage = creature.basicStatus.magicalAttack;
            attack.damageType = IDamaged.DamageType.Magical;
        }

        switch (creature.basicStatus.targettingType)
        {
            case IGetTarget.TargettingType.AllInRange:
                targetingType = gameObject.AddComponent<AllInRange>();
                break;
            case IGetTarget.TargettingType.LowestHp:
                targetingType = gameObject.AddComponent<SortingHp>();
                break;
            case IGetTarget.TargettingType.HighestAtk:
                targetingType = gameObject.AddComponent<SortingAtk>();
                break;
        }
    }
    public void Attack()
    {
        if (targetingType == null)
            return;
        
        if (targetingType is AllInRange)
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
