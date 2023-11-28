using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 destinationPos;
    private Vector2 initPos;
    private float duration;
    private float destroyTime;
    private float projectileHeight;

    private bool isShoot = false;
    
    public void SetData(SOBasicStatus status)
    {
        initPos = gameObject.transform.position;
        duration = status.projectileDuration;
        destinationPos = initPos;
        destinationPos.x += status.AttackRange;
        Destroy(gameObject, duration);
        destroyTime = Time.time + duration;
        projectileHeight = status.projectileHeight;
        isShoot = true;
    }

    public void SetTargetPos(Creature target)
    {
        destinationPos = target.transform.position;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag(collision.gameObject.tag))
            return;
        AttackInfo atk = new();
        collision.GetComponent<IDamagable>().OnDamaged(atk);
    }
}
