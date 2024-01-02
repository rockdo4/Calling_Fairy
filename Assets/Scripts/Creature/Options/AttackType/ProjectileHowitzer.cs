using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHowitzer : Projectile
{
    protected float rangeFactor = 1f;
    public override void SetTargetPos(Creature target)
    {
        destinationPos = target.transform.position;
        rangeFactor = Vector2.Distance(destinationPos, initPos) / maxRange;
        duration *= rangeFactor;
        destroyTime = Time.time + duration;
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

        var prePos = Vector2.Lerp(destinationPos, initPos, (destroyTime - Time.time) / duration);
        prePos.y += Mathf.Sin((destroyTime - Time.time) / duration * Mathf.PI) * projectileHeight;
        var lookingAt = prePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookingAt.y, lookingAt.x) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(prePos, Quaternion.AngleAxis(angle, Vector3.forward));

        if (destroyTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
