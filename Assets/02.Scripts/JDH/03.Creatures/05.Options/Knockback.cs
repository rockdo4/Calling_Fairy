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
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        var xPos = creature.basicStatus.basicMoveSpeed > 0 ? -attack.knockbackDistance : attack.knockbackDistance;
        var vec = new Vector2(xPos,0);
        vec.x *= 1 - creature.basicStatus.knockbackResist;
        rb.AddForce(vec);
    }
}
