using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttackType
{
    private const string Key = "Projectile";
    private Creature creature;
    private AttackInfo attack;
    private void Awake()
    {
        creature = GetComponent<Creature>();
        attack.attacker = creature.gameObject;
        attack.knockbackDistance = creature.Status.knockbackDistance;
        attack.accuracy = creature.Status.accuracy;
        attack.damage = creature.Status.damage;
        attack.damage *= creature.Status.attackFactor;
        attack.damageType = creature.Status.damageType;
        attack.attackType = AttackType.Projectile;
        attack.effectType = EffectType.ProjectileAttack;
    }
    public void Attack()
    {

        foreach (var target in creature.targets)
        {
            if (target == null)
            {
                return;
            }
        }
        //���� �߰��ؾ���.
        if (creature.normalAttackSE != null)
        {
            AudioManager.Instance.PlaySE(creature.normalAttackSE);
        }
        var projectile = Instantiate(Key.GetGo(false), creature.Rigidbody.worldCenterOfMass, Quaternion.identity);
        //var projectile = Instantiate(creature.stageManager.projectile, creature.transform.position, Quaternion.identity);
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
