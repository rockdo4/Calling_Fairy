using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamagable;

public class DamagedTest : MonoBehaviour, IDamagable
{
    public void OnDamage(GameObject deffender, float Damage, DamageType damagaType)
    {
        Debug.Log("damaged!");
    }
}
