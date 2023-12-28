using UnityEngine;

public class SkillProjectileHowitzer : ProjectileHowitzer
{
    AttackInfo[] atks;
    
    public override void SetData(in SkillProjectileData sp ,in AttackInfo[] attackInfos, in SkillData skillData)
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(sp.proj_sprite);
        sr.color = Color.white;
        transform.localScale = Vector3.one;

        gameObject.AddComponent<BoxCollider2D>();
        maxRange = skillData.skill_range;
        atks = attackInfos;
        initPos = transform.position;
        duration = sp.proj_life;
        projectileHeight = sp.proj_highest;
        destroyTime = Time.time + duration;
        isShoot = true;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var scripts = collision.GetComponents<IDamagable>();
        foreach(var atk in atks)
        {
            if (atk.targetingType == TargetingType.Enemy)
            {
                if (!collision.CompareTag(atk.attacker.tag))
                {
                    foreach (var script in scripts)
                    {
                        script.OnDamaged(atk);
                    }
                }
            }
            else
            {
                if (collision.CompareTag(atk.attacker.tag))
                {
                    foreach (var script in scripts)
                    {
                        script.OnDamaged(atk);
                    }
                }
            }
        }    
    }        
}