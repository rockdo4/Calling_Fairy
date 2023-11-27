using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectile : MonoBehaviour
{
    private float speed;
    private float destroyTime;
    
    public void SetData()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackInfo atk = new();
        collision.GetComponent<IDamagable>().OnDamaged(atk);
    }
}
