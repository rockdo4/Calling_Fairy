using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 destinationPos;
    private Vector2 initPos;
    private float duration;
    private float destroyTime;
    private float projectileHeight;
    private float maxRange;
    private float rangeFactor = 1f;
    AttackInfo atk;

    private bool isShoot = false;
    
    public void SetData(IngameStatus status, AttackInfo attackInfo)
    {
        maxRange = status.attackRange;
        atk = attackInfo;
        initPos = transform.position;
        duration = status.projectileDuration;
        projectileHeight = status.projectileHeight;
        destroyTime = Time.time + duration;
        isShoot = true;
    }
    public void SetData(SOSkillInfo status, AttackInfo attackInfo)
    {
        atk = attackInfo;
        maxRange = status.range;
        initPos = transform.position;
        duration = status.projectileDuration;
        projectileHeight = status.projectileHeight;
        destroyTime = Time.time + duration;
        destinationPos = initPos;
        destinationPos.x += status.range;
        isShoot = true;
    }

    public void SetTargetPos(Creature target)
    {
        destinationPos = target.transform.position;        
        rangeFactor = Vector2.Distance(destinationPos, initPos) / maxRange;
        duration *= rangeFactor;
        projectileHeight *= rangeFactor;
        destroyTime = Time.time + duration;
    }

    private void Update()
    {
        if (!isShoot)
            return;
        if (duration == 0)
            return;

        var prePos = Vector2.Lerp(destinationPos, initPos, (destroyTime - Time.time) / duration); 
        if(projectileHeight != 0)
        {
            prePos.y += Mathf.Sin((destroyTime - Time.time) / duration * Mathf.PI) * projectileHeight;
        }
        var lookingAt = prePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookingAt.y, lookingAt.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = prePos;

        if(destroyTime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag(collision.gameObject.tag))
             return;
        var scripts = collision.GetComponents<IDamagable>();
        foreach(var script in scripts)
        {
            script.OnDamaged(atk);
        }
    }        
}