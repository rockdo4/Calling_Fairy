using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttackType
{
    private Creature creature;
    private AttackInfo attack;
    private void Awake()
    {
        creature = GetComponent<Creature>();
        attack.attacker = creature.gameObject;
        attack.knockbackDistance = creature.Status.knockbackDistance;
        attack.accuracy = creature.Status.accuracy;
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
        var projectile = Instantiate(creature.stageManager.projectile, gameObject.transform.position, Quaternion.identity);
        projectile.layer = gameObject.layer;
        projectile.tag = creature.gameObject.tag;
        var script = projectile.AddComponent<Projectile>();
        script.SetData(creature.Status, attack);
        script.SetTargetPos(creature.targets[0]);
    }
}
