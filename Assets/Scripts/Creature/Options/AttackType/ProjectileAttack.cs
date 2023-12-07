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
        attack.damage = creature.Status.damage;
        attack.damageType = creature.Status.damageType;
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
