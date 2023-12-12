using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectileDirect : ProjectileDirect
{
    AttackInfo[] atks;

    public override void SetData(in SkillProjectileData sp, in AttackInfo[] attackInfos, in SkillData skillData)
    {
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
}
