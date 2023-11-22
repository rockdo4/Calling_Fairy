using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using static IDamaged;

public class Creature : MonoBehaviour, IDamagable
{
    public SOBasicStatus basicStatus;
    public float AttackDamageFactor;
    public Rigidbody2D Rigidbody { get; private set; }
    private CreatureController CC;
    public IDamagable target;
    public float curHP;
    public InGameManager inGameManager;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        inGameManager = GameObject.FindWithTag(Tags.InGameManager).GetComponent<InGameManager>();
        curHP = basicStatus.hP;
    }

    private void FixedUpdate()
    {
        CC.curState.OnFixedUpdate();
    }

    private void Update()
    {
        CC.curState.OnUpdate();        
    }

    public void OnDamaged(float Damage, DamageType damageType)
    {
        var damagedStripts = GetComponents<IDamaged>();
        foreach(var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, Damage, damageType);
        }
    }

    public void OnDestructed()
    {
        var destuctScripts = GetComponents<IDestructable>();
        foreach(var destuctScript in destuctScripts)
        {
            destuctScript.OnDestructed();
        }
    }
}
