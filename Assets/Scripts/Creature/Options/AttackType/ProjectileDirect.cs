using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDirect : Projectile
{
    protected float speed;
    public override void SetTargetPos(Creature target)
    {
        destinationPos = target.transform.position;
        destinationPos.y = 0;
        destinationPos.Normalize();
        speed = maxRange / duration;
    }

    protected void Update()
    {
        if (!isShoot)
            return;
        if (duration == 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += speed * Time.deltaTime * (Vector3)destinationPos;

        if (destroyTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
