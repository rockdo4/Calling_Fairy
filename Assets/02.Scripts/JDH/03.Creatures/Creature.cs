using System.Collections;
using UnityEngine;
using static IDamaged;

public class Creature : MonoBehaviour, IDamagable
{
    public SOBasicStatus basicStatus;
    public float AttackDamageFactor;
    public Rigidbody2D Rigidbody { get; private set; }
    private CreatureController CC;
    public IDamagable target;
    public float curHP;
    public StageManager stageManager;
    public bool isAttacked = false;
    public bool isDead = false;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
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

    public void OnDamaged(float damage, DamageType damageType)
    {
        var damagedStripts = GetComponents<IDamaged>();
        foreach(var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, damage, damageType);
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
        target = null;
        isAttacked = false;
    }
}
