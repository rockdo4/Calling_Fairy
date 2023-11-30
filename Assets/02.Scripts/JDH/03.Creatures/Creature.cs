using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Creature : MonoBehaviour, IDamagable
{
    //dummyData
    [SerializeField]
    public SOBasicStatus basicStatus;
    [SerializeField]
    protected SOSkillInfo[] TestSkills;

    protected bool isLoaded = false;

    //public Image HpBackGround;
    //public Image HpBar;
    public Slider HpBar;
    public Rigidbody2D Rigidbody { get; private set; }
    public CreatureController CC;
    public List<Creature> targets = new();
    public float curHP;
    public StageManager stageManager;
    public bool isAttacking = false;
    public bool isDead = false;

    protected Stack<SkillBase> skills = new();
    protected event Action NormalSkill;
    protected event Action ReinforcedSkill;
    protected event Action SpecialSkill;

    protected IAttackType.AttackType attackType;
    public GetTarget.TargettingType targettingType;
    public IAttackType attack;
    public GetTarget getTarget;

    public GameObject projectile = null;
    public LinkedList<BuffBase> buffs = new();
    public IngameStatus Status
    {
        get { return returnStatus; }
        private set { returnStatus = value; }
    }
    public IngameStatus MultipleStatus
    {
        get { return multipleStatus; }
        set 
        {
            multipleStatus = value;
            returnStatus = (realStatus + plusStatus) * multipleStatus;
        }
    }
    public IngameStatus PlusStatus
    {
        get { return plusStatus; }
        set 
        {
            plusStatus = value;
            returnStatus = (realStatus + plusStatus) * multipleStatus;
        }
    }
    protected IngameStatus plusStatus;
    protected IngameStatus multipleStatus = new(IngameStatus.MakeType.Multiple);
    protected IngameStatus realStatus;
    protected IngameStatus returnStatus;


    protected virtual void Awake()
    {
        TryGetComponent<Fairy>(out var fairyObject);
        if (fairyObject is not Fairy)
            SetData();
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        

        getTarget = targettingType switch
        {
            GetTarget.TargettingType.AllInRange => new AllInRange(),
            GetTarget.TargettingType.SortingAtk => new SortingAtk(),
            GetTarget.TargettingType.SortingHp => new SortingHp(),
            _ => null
        };
    }

    protected void Start()
    {
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
    }

    private void FixedUpdate()
    {
        CC.curState.OnFixedUpdate();
        
    }

    private void Update()
    {
        //testCode
        if(Input.GetKeyDown(KeyCode.Z))
        {
            NormalSkill?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            ReinforcedSkill?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            SpecialSkill?.Invoke();
        }

        CC.curState.OnUpdate();
        foreach (var buff in buffs)
        {
            buff.OnUpdate();
        }
    }

    public void OnDamaged(AttackInfo attack)
    {
        if (UnityEngine.Random.value > attack.accuracy - Status.evasion)        
             return;
        
        var damagedStripts = GetComponents<IDamaged>();
        foreach (var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, attack);
        }
        if(attack.buffInfo.HasInfo)
        {
            GetBuff(attack.buffInfo);
        }
        LerpHpUI();
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
        yield return new WaitForSeconds(1 / Status.attackSpeed);
        isAttacking = false;
    }

    public void Attack()
    {
        if (isAttacking)
            return;
        StartCoroutine(AttackTimer());
        getTarget?.FilterTarget(ref targets);
        attack.Attack();
    }

    public void GetBuff(BuffInfo buffInfo)
    {
        var buff = BuffBase.MakeBuff(buffInfo.buffType);
        buff.SetBuff(buffInfo);
        buff.OnEnter();
        buffs.AddFirst(buff);
    }
    public void SetData()
    {
        if(isLoaded)
            return;
        realStatus.hp = basicStatus.hp;
        realStatus.physicalAttack = basicStatus.physicalAttack;
        realStatus.magicalAttack = basicStatus.magicalAttack;
        realStatus.physicalArmor = basicStatus.physicalArmor;
        realStatus.magicalArmor = basicStatus.magicalArmor;
        realStatus.criticalChance = basicStatus.criticalChance;
        realStatus.criticalFactor = basicStatus.criticalFactor;
        realStatus.evasion = basicStatus.evasion;
        realStatus.accuracy = basicStatus.accuracy;
        realStatus.attackSpeed = basicStatus.attackSpeed;
        realStatus.attackRange = basicStatus.attackRange;
        realStatus.basicMoveSpeed = basicStatus.basicMoveSpeed;
        realStatus.moveSpeed = basicStatus.moveSpeed;
        realStatus.knockbackDistance = basicStatus.KnockbackDistance;
        realStatus.knockbackResist = basicStatus.knockbackResist;
        realStatus.attackFactor = basicStatus.attackFactor;
        realStatus.projectileDuration = basicStatus.projectileDuration;
        realStatus.projectileHeight = basicStatus.projectileHeight;
        attackType = basicStatus.attackType;
        targettingType = basicStatus.targettingType;
        returnStatus = realStatus;


        foreach (var testSkill in TestSkills)
        {
            var skill = SkillBase.MakeSkill(testSkill, this);
            skills.Push(skill);
            switch (testSkill.ID % 100)
            {
                case 1:
                    NormalSkill += skill.Active;
                    break;
                case 2:
                    ReinforcedSkill += skill.Active;
                    break;
                case 3:
                    SpecialSkill += skill.Active;
                    break;
                default:
                    break;
            }
        }
        curHP = Status.hp;
    }
    public void LerpHpUI()
    {
        HpBar.value = curHP / Status.hp;
    }

    public void Die()
    {
        CC.ChangeState(StateController.State.Dead);
    }

    public void ActiveNormalSkill()
    {
        NormalSkill.Invoke();
    }
    public void ActiveReinforcedSkill()
    {
        ReinforcedSkill.Invoke();
    }
    public void ActiveSpecialSkill()
    {
        //SpecialSkill.Invoke();
    }
}
