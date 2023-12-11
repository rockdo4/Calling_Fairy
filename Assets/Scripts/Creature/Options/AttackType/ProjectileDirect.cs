using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDirect : Projectile
{
    protected float speed;
    public override void SetTargetPos(Creature target)
    {
        destinationPos = target.transform.position;
        destinationPos.Normalize();
        speed = maxRange / duration;
    }

    private void Update()
    {
        if (!isShoot)
            return;
        if (duration == 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)destinationPos * speed * Time.deltaTime;

        if (destroyTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
