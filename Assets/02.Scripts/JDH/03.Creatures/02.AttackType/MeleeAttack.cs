using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackType
{
    private AttackInfo attack;
    private Creature creature;
    private readonly List<IDamagable> targets = new();

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
    }
    public void Attack()
    {
        targets.Clear();
        var allTargets = Physics2D.OverlapCircleAll(creature.transform.position, creature.basicStatus.AttackRange);
        foreach (var target in allTargets)
        {
            var targetCreature = target.GetComponent<IDamagable>();
            if (targetCreature == null || target.gameObject.layer == creature.gameObject.layer)
                continue;
            targets.Add(targetCreature);
        }

        foreach(var target in targets)
        {
            target.OnDamaged(attack);
        }
    }
}
