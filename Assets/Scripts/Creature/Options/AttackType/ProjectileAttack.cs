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
        Projectile script;
        if(creature.Status.projectileHeight != 0)
        {
            script = projectile.AddComponent<ProjectileHowitzer>();
        }
        else
        {
            script = projectile.AddComponent<ProjectileDirect>();
        }        
        script.SetData(creature.Status, attack);
        foreach(var target in creature.targets)
        {
            if(target == null)
            {
                continue;
            }
            script.SetTargetPos(target);
            break;
        }
    }
}
