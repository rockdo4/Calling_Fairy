using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamaged;

public class Creature : MonoBehaviour, IDamagable
{
    public SOBasicStatus basicStatus;
    public float AttackDamageFactor;
    public Rigidbody2D Rigidbody { get; private set; }
    private CreatureController CC;   
    public List<IDamagable> targets;
    public float curHP;
    public StageManager stageManager;
    public bool isAttacked = false;
    public bool isDead = false;
    public IAttackType attack;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        curHP = basicStatus.hP;

        switch (basicStatus.attackType)
        {
            case IAttackType.AttackType.Melee:
                attack = gameObject.AddComponent<MeleeAttack>();
                break;
            case IAttackType.AttackType.DirectProjectile:
                attack = gameObject.AddComponent<DirectProjectileAttack>();
                break;
            case IAttackType.AttackType.HowitzerProjectile:
                attack = gameObject.AddComponent<HowitzerProjectileAttack>();
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()
    {
        CC.curState.OnFixedUpdate();
    }

    private void Update()
    {
        CC.curState.OnUpdate();        
    }

    public void OnDamaged(AttackInfo attack)
    {
        var damagedStripts = GetComponents<IDamaged>();
        foreach(var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, attack);
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

    public void StartAttackTimer()
    {
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer()
    {
        isAttacked = true;
        yield return new WaitForSeconds(basicStatus.AttackSpeed);
        targets = null;
        isAttacked = false;
    }
    
    public void Attack()
    {
        attack.Attack();
    }
}