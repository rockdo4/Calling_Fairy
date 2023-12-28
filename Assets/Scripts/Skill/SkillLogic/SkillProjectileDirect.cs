using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectileDirect : ProjectileDirect
{
    private AttackInfo[] atks;
    private float accel;

    public override void SetData(in SkillProjectileData sp, in AttackInfo[] attackInfos, in SkillData skillData)
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(sp.proj_sprite);
        sr.color = Color.white;
        transform.localScale = Vector3.one;

        maxRange = skillData.skill_range;
        atks = attackInfos;
        initPos = transform.position;
        duration = sp.proj_life;
        projectileHeight = sp.proj_highest;
        destroyTime = Time.time + duration;
        speed = sp.proj_speed / 100;
        accel = sp.proj_acceleration;
        isShoot = true;        
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var scripts = collision.GetComponents<IDamagable>();
        foreach (var atk in atks)
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
    protected void Update()
    {
        if (!isShoot)
            return;
        if (duration == 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += new Vector3(speed * Time.deltaTime, 0);
        speed += accel * Time.deltaTime;
        if (destroyTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
