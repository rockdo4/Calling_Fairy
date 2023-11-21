using System.Collections.Generic;
using UnityEngine;
using static IDamagable;

public class Creature : MonoBehaviour, ITargetable
{
    public SOBasicStatus basicStatus;
    public float AttackDamageFactor;
    public Rigidbody2D Rigidbody { get; private set; }
    private CreatureController CC;
    public ITargetable target;

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
    }

    private void FixedUpdate()
    {
        CC.curState.OnFixedUpdate();
    }

    private void Update()
    {
        CC.curState.OnUpdate();        
    }

    public void OnTargeted(float Damage, DamageType damageType)
    {
        var damageds = GetComponents<IDamagable>();
        foreach(var damaged in damageds)
        {
            damaged.OnDamage(gameObject, Damage, damageType);
        }
    }
}
