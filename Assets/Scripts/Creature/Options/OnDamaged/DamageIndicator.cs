using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private InGameEffectPool pool;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        pool = GameObject.FindWithTag(Tags.EffectPool).GetComponent<InGameEffectPool>();
    }
    public void IndicateDamage(DamageType damageType, float damage, bool isCritical, bool isAvoid, bool isHeal = false)
    {
        if(col == null)
        {
            return;
        }
        var bound = col.bounds;        
        var PosX = Random.Range(bound.min.x, bound.max.x - (bound.size.x / 2));
        var PosY = Random.Range(bound.min.y + (bound.size.y / 3 * 2), bound.max.y);
        if(isCritical)
        {
            PosY += bound.size.y / 3;
        }
        var position = new Vector3(PosX, PosY, 0);
        var gameObject = pool.GetEffect(EffectType.String);
        var effect = gameObject.GetComponent<DamageDataEffects>();
        effect.SetPositionAndRotation(position);
        if(isAvoid)
        {
            effect.SetDamage(DamageDataEffects.InfoType.Avoid);
            return;
        }
        if(isHeal)
        {
            effect.SetDamage(DamageDataEffects.InfoType.Heal, damage.ToString());
            return;
        }
        effect.SetDamage(DamageDataEffects.InfoType.Heal, damage.ToString());
        switch (damageType)
        {
            case DamageType.Physical:
                if(!isCritical)
                    effect.SetDamage(DamageDataEffects.InfoType.Physical, damage.ToString());
                else
                    effect.SetDamage(DamageDataEffects.InfoType.CriticalPhysical, damage.ToString());
                break;
            case DamageType.Magical:
                if(!isCritical)
                    effect.SetDamage(DamageDataEffects.InfoType.Magical, damage.ToString());
                else
                    effect.SetDamage(DamageDataEffects.InfoType.CriticalMagical, damage.ToString());
                break;
            default:
                break;
        }        
    }
}
