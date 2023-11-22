using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public class Knockback : MonoBehaviour, IDamaged
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnDamage(GameObject deffender, float Damage, DamageType damagaType)
    {
        //rb.AddForce()
    }
}
