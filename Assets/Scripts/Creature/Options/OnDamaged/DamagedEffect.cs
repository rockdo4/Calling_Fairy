using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        var effectGameObject = pool.GetEffect(effectType);
        var effect = effectGameObject.GetComponent<Effects>();
        var defScript = deffender.GetComponent<Creature>();
        effect.SetPositionAndRotation(new Vector2(0, defScript.CenterPos.y) + (Vector2)gameObject.transform.position);
    }
}
