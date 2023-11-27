using UnityEngine;

public class DirectProjectileAttack : MonoBehaviour, IAttackType
{
    public GameObject projectilePrefab;
    public void Attack()
    {
        var projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.layer = gameObject.layer;
    }
}
