using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public class Knockback : MonoBehaviour, IDamaged
{
    private Rigidbody2D rb;
    private Creature creature;

    private void Awake()
    {
        creature = GetComponent<Creature>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnDamage(GameObject deffender, float damage, DamageType damagaType)
    {
        var xPos = creature.basicStatus.moveSpeed > 0 ? -1 : 1;
        var vec = new Vector2(xPos,0);
        //rb.AddForce(vec * damage);
        GetComponent<Rigidbody2D>().AddForce(vec * damage);
    }
}
