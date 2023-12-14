using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedEffect : MonoBehaviour, IDamaged
{
    private InGameEffectPool pool;

    private void Awake()
    {
        pool = GameObject.FindWithTag(Tags.EffectPool).GetComponent<InGameEffectPool>();
    }
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        EffectType effectType;
        if (attack.attackType == AttackType.Melee)
        {
            effectType = EffectType.MeleeAttack;
        }
        else
        {
            effectType = EffectType.ProjectileAttack;
        }
        var gameObject = pool.GetEffect(effectType);
        var effect = gameObject.GetComponent<Effects>();
        effect.SetPositionAndRotation(deffender.transform.position);
    }
}
