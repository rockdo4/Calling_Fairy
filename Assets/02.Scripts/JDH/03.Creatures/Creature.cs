using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IDamagable
{
    //dummyData
    [SerializeField] 
    private SOBasicStatus basicStatus;

    public Rigidbody2D Rigidbody { get; private set; }
    protected CreatureController CC;
    public List<Creature> targets = new();
    public float curHP;
    public StageManager stageManager;
    public bool isAttacking = false;
    public bool isDead = false;


    public IAttackType.AttackType attackType;
    public GetTarget.TargettingType targettingType;
    public IAttackType attack;
    public GetTarget getTarget;

    public GameObject projectile = null;
    public LinkedList<BuffBase> buffs = new();
    public IngameStatus Status
    {
        get
        {
            return (realStatus + plusStatus) * multipleStatus;
        }
    }
    public IngameStatus plusStatus;
    public IngameStatus multipleStatus = new(IngameStatus.MakeType.Multiple);
    private IngameStatus realStatus;


    protected virtual void Awake()
    {
        SetData();
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        curHP = Status.hp;

        switch (attackType)
        {
            case IAttackType.AttackType.Melee:
                attack = gameObject.AddComponent<MeleeAttack>();
                break;
            case IAttackType.AttackType.Projectile:
                attack = gameObject.AddComponent<ProjectileAttack>();
                break;
            default:
                break;
        }

        getTarget = targettingType switch
        {
            GetTarget.TargettingType.AllInRange => new AllInRange(),
            GetTarget.TargettingType.SortingAtk => new SortingAtk(),
            GetTarget.TargettingType.SortingHp => new SortingHp(),
            _ => null
        };
    }
    private void FixedUpdate()
    {
        CC.curState.OnFixedUpdate();
    }

    private void Update()
    {
        CC.curState.OnUpdate();
        foreach (var buff in buffs)
        {
            buff.OnUpdate();
        }
    }

    public void OnDamaged(AttackInfo attack)
    {
        var damagedStripts = GetComponents<IDamaged>();
        foreach (var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, attack);
        }
    }

    public void OnDestructed()
    {
        var destuctScripts = GetComponents<IDestructable>();
        foreach (var destuctScript in destuctScripts)
        {
            destuctScript.OnDestructed();
        }
    }

    private IEnumerator AttackTimer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1 / basicStatus.attackSpeed);
        isAttacking = false;
    }

    public void Attack()
    {
        StartCoroutine(AttackTimer());
        getTarget?.FilterTarget(ref targets);
        attack.Attack();
    }

    public void SetData()
    {
        realStatus.hp = basicStatus.hp;
        realStatus.physicalAttack = basicStatus.physicalAttack;
        realStatus.magicalAttack = basicStatus.magicalAttack;
        realStatus.physicalArmor = basicStatus.physicalArmor;
        realStatus.magicalArmor = basicStatus.magicalArmor;
        realStatus.criticalChance = basicStatus.criticalChance;
        realStatus.criticlaFactor = basicStatus.criticlaFactor;
        realStatus.evasion = basicStatus.evasion;
        realStatus.accuracy = basicStatus.accuracy;
        realStatus.attackSpeed = basicStatus.attackSpeed;
        realStatus.attackRange = basicStatus.attackRange;
        realStatus.basicMoveSpeed = basicStatus.basicMoveSpeed;
        realStatus.moveSpeed = basicStatus.moveSpeed;
        realStatus.KnockbackDistance = basicStatus.KnockbackDistance;
        realStatus.knockbackResist = basicStatus.knockbackResist;
        realStatus.attackFactor = basicStatus.attackFactor;
        realStatus.projectileDuration = basicStatus.projectileDuration;
        realStatus.projectileHeight = basicStatus.projectileHeight;
        attackType = basicStatus.attackType;
        targettingType = basicStatus.targettingType;
    }
}

public struct IngameStatus
{
    public enum MakeType
    {
        Normal,
        Multiple,
    }

    public float hp;
    public float physicalAttack;
    public float magicalAttack;
    public float physicalArmor;
    public float magicalArmor;
    public float criticalChance;
    public float criticlaFactor;
    public float evasion;
    public float accuracy;
    public float attackSpeed;
    public float attackRange;
    public float basicMoveSpeed;
    public float moveSpeed;
    public float KnockbackDistance;
    public float knockbackResist;
    public float attackFactor;
    public float projectileDuration;
    public float projectileHeight;

    public IngameStatus(MakeType make = MakeType.Normal)
    {
        if(make == MakeType.Multiple) 
        {
            hp = 1f;
            physicalAttack = 1f;
            magicalAttack = 1f;
            physicalArmor = 1f;
            magicalArmor = 1f;
            criticalChance = 1f;
            criticlaFactor = 1f;
            evasion = 1f;
            accuracy = 1f;
            attackSpeed = 1f;
            attackRange = 1f;
            basicMoveSpeed = 1f;
            moveSpeed = 1f;
            KnockbackDistance = 1f;
            knockbackResist = 1f;
            attackFactor = 1f;
            projectileDuration = 1f;
            projectileHeight = 1f;
        }
        else
        {
            hp = 0f;
            physicalAttack = 0f;
            magicalAttack = 0f;
            physicalArmor = 0f;
            magicalArmor = 0f;
            criticalChance = 0f;
            criticlaFactor = 0f;
            evasion = 0f;
            accuracy = 0f;
            attackSpeed = 0f;
            attackRange = 0f;
            basicMoveSpeed = 0f;
            moveSpeed = 0f;
            KnockbackDistance = 0f;
            knockbackResist = 0f;
            attackFactor = 0f;
            projectileDuration = 0f;
            projectileHeight = 0f;
        }
    }

    public static IngameStatus operator +(IngameStatus lhs, IngameStatus rhs)
    {
        IngameStatus rtn;
        rtn.hp = lhs.hp + rhs.hp;
        rtn.physicalAttack = lhs.physicalAttack + rhs.physicalAttack;
        rtn.magicalAttack = lhs.magicalAttack + rhs.magicalAttack;
        rtn.physicalArmor = lhs.physicalArmor + rhs.physicalArmor;
        rtn.magicalArmor = lhs.magicalArmor + rhs.magicalArmor;
        rtn.criticalChance = lhs.criticalChance + rhs.criticalChance;
        rtn.criticlaFactor = lhs.criticlaFactor + rhs.criticlaFactor;
        rtn.evasion = lhs.evasion + rhs.evasion;
        rtn.accuracy = lhs.accuracy + rhs.accuracy;
        rtn.attackSpeed = lhs.attackSpeed + rhs.attackSpeed;
        rtn.attackRange = lhs.attackRange + rhs.attackRange;
        rtn.basicMoveSpeed = lhs.basicMoveSpeed + rhs.basicMoveSpeed;
        rtn.moveSpeed = lhs.moveSpeed + rhs.moveSpeed;
        rtn.KnockbackDistance = lhs.KnockbackDistance + rhs.KnockbackDistance;
        rtn.knockbackResist = lhs.knockbackResist + rhs.knockbackResist;
        rtn.attackFactor = lhs.attackFactor + rhs.attackFactor;
        rtn.projectileDuration = lhs.projectileDuration + rhs.projectileDuration;
        rtn.projectileHeight = lhs.projectileHeight + rhs.projectileHeight;
        return rtn;
    }
    public static IngameStatus operator *(IngameStatus lhs, IngameStatus rhs)
    {
        IngameStatus rtn;
        rtn.hp = lhs.hp * rhs.hp;
        rtn.physicalAttack = lhs.physicalAttack * rhs.physicalAttack;
        rtn.magicalAttack = lhs.magicalAttack * rhs.magicalAttack;
        rtn.physicalArmor = lhs.physicalArmor * rhs.physicalArmor;
        rtn.magicalArmor = lhs.magicalArmor * rhs.magicalArmor;
        rtn.criticalChance = lhs.criticalChance * rhs.criticalChance;
        rtn.criticlaFactor = lhs.criticlaFactor * rhs.criticlaFactor;
        rtn.evasion = lhs.evasion * rhs.evasion;
        rtn.accuracy = lhs.accuracy * rhs.accuracy;
        rtn.attackSpeed = lhs.attackSpeed * rhs.attackSpeed;
        rtn.attackRange = lhs.attackRange * rhs.attackRange;
        rtn.basicMoveSpeed = lhs.basicMoveSpeed * rhs.basicMoveSpeed;
        rtn.moveSpeed = lhs.moveSpeed * rhs.moveSpeed;
        rtn.KnockbackDistance = lhs.KnockbackDistance * rhs.KnockbackDistance;
        rtn.knockbackResist = lhs.knockbackResist * rhs.knockbackResist;
        rtn.attackFactor = lhs.attackFactor * rhs.attackFactor;
        rtn.projectileDuration = lhs.projectileDuration * rhs.projectileDuration;
        rtn.projectileHeight = lhs.projectileHeight * rhs.projectileHeight;
        return rtn;
    }
}