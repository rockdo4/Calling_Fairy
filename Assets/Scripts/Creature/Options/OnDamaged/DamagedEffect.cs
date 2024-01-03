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
        if (attack.effectType == EffectType.None)
            return;
        var effectGameObject = pool.GetEffect(attack.effectType);
        //test
        if( attack.effectType != EffectType.MeleeAttack && attack.effectType != EffectType.ProjectileAttack)
        {
            Debug.Log(attack.effectType.ToString());
        }
        if(attack.effectType == EffectType.SkillNormalTarget2_0 || attack.effectType == EffectType.SkillNormalTarget2_1 || attack.effectType == EffectType.SkillReinforceTarget2_1)
        {
            Debug.LogWarning("effect");
        }        
        //test
        if (effectGameObject == null)
            return;
        var effect = effectGameObject.GetComponent<Effects>();
        var defScript = deffender.GetComponent<Creature>();
        effect.SetPositionAndRotation(defScript.Rigidbody.centerOfMass + (Vector2)gameObject.transform.position, defScript is Fairy);                
    }
}
