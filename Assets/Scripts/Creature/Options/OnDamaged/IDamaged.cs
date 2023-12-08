using UnityEngine;

public interface IDamaged
{    
    public void OnDamage(GameObject deffender, AttackInfo attack);
}