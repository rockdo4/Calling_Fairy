using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector2 destinationPos;
    protected Vector2 initPos;
    protected float duration;
    protected float destroyTime;
    protected float projectileHeight;
    protected float maxRange;
    protected Creature attacker;
    AttackInfo[] atks;

    protected bool isShoot = false;
    
    public void SetData(IngameStatus status, in AttackInfo attackInfo)
    {
        atks = new AttackInfo[1];
        maxRange = status.attackRange;
        atks[0] = attackInfo;
        initPos = transform.position;
        duration = status.projectileDuration;
        projectileHeight = status.projectileHeight;
        destroyTime = Time.time + duration;
        isShoot = true;
        attacker = atks[0].attacker.GetComponent<Creature>();
    }
    public virtual void SetData(in SkillProjectileData sp, in AttackInfo[] attackInfos, in SkillData skillData) { }

    public abstract void SetTargetPos(Creature target);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag(collision.gameObject.tag))
             return;
        var scripts = collision.GetComponents<IDamagable>();

        if (Random.value < attacker.Status.criticalChance)
        {
            var criticalAttack = atks;
            for(int i = 0; i < criticalAttack.Length; i++)
            {
                criticalAttack[i].damage *= attacker.Status.criticalFactor;
                criticalAttack[i].isCritical = true;
                foreach (var script in scripts)
                {
                    script.OnDamaged(criticalAttack[i]);
                }
            }
        }
        else
        {
            foreach (var atk in atks)
            {
                foreach (var script in scripts)
                {
                    script.OnDamaged(atk);
                }
            }
        }
    }        
}