using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveJump : MonoBehaviour, IDestructable
{
    [SerializeField]
    private float minForce = 20f;
    [SerializeField]
    private float maxForce = 30f;
    [SerializeField]
    [Range(0, 90)]
    private float minAngle = 20f;
    [SerializeField]
    [Range(0, 90)]
    private float maxAngle = 70f;
    [SerializeField]
    private float rollingRate = 300f;

    [SerializeField]
    private SOExplosiveJump explosiveJumpInfo;

    private Rigidbody2D rb;
    private Creature creature;

    private void Awake()
    {
        creature = GetComponent<Creature>();
        rb = GetComponent<Rigidbody2D>();
        if (explosiveJumpInfo == null)
            return;
        minForce = explosiveJumpInfo.minForce;
        maxForce = explosiveJumpInfo.maxForce;
        minAngle = explosiveJumpInfo.minAngle;
        maxAngle = explosiveJumpInfo.maxAngle;
        rollingRate = explosiveJumpInfo.rollingRate;
    }

    public void OnDestructed()
    {
        if (!creature.isDeadWithSkill)
            return;
        float angle = Random.Range(minForce, maxForce);
        Vector2 dir = new(1, Mathf.Cos(Mathf.Deg2Rad * angle));
        dir.Normalize();
        float force = Random.Range(minForce, maxForce);
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        rb.rotation = rollingRate;
    }
}
