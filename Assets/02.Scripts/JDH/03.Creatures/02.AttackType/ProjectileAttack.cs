using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttackType
{
    private Creature creature;
    private void Awake()
    {
        creature = GetComponent<Creature>();
    }
    public void Attack()
    {
        var projectile = Instantiate(creature.projectile, gameObject.transform.position, Quaternion.identity);
        projectile.layer = gameObject.layer;
        var script = projectile.AddComponent<Projectile>();
        script.SetTargetPos(creature.targets[0]);
        script.SetData(creature.Status);
    }
}
