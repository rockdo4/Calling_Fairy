using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborne : MonoBehaviour, IDamaged
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnDamage(GameObject deffender, AttackInfo attack)
    {
        if (deffender.GetComponent<Creature>().isDead)
            return;
        var vec = new Vector2(0, attack.airborneDistance);
        rb.AddForce(vec);
    }
}
