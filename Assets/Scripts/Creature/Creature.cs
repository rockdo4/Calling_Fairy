using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Creature : MonoBehaviour, IDamagable
{
    protected bool isLoaded = false;

    //public Image HpBackGround;
    //public Image HpBar;
    protected Slider HpBar;
    public Rigidbody2D Rigidbody { get; private set; }
    protected CreatureController CC;
    public List<Creature> targets = new();
    public float curHP;
    public StageManager stageManager;
    public bool isAttacking = false;
    public bool isDead = false;

    protected Stack<SkillBase> skills = new();
    protected event Action NormalSkill;
    protected event Action ReinforcedSkill;
    protected event Action SpecialSkill;

    protected Queue<Action> skillQueue = new();
    protected bool isSkillUsing = false;

    [HideInInspector]
    public AttackType attackType;
    [HideInInspector]
    public GetTarget.TargettingType targettingType;
    [HideInInspector]
    public IAttackType attack;
    protected GetTarget getTarget;
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
        Rigidbody = GetComponent<Rigidbody2D>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        gameObject.AddComponent<Knockback>();
        gameObject.AddComponent<Airborne>();
        gameObject.AddComponent<Die>();
        gameObject.AddComponent<Damaged>();
        getTarget = targettingType switch
        {
            GetTarget.TargettingType.AllInRange => new AllInRange(),
            GetTarget.TargettingType.SortingAtk => new SortingAtk(),
            GetTarget.TargettingType.SortingHp => new SortingHp(),
            _ => null
        };
        HpBar = GetComponentInChildren<Slider>();
    }

    protected virtual void Start()
    {
        curHP = Status.hp;
        LerpHpUI();
        switch (attackType)
        {
            case AttackType.Melee:
                attack = gameObject.AddComponent<MeleeAttack>();
                break;
            case AttackType.Projectile:
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
        CC.curState.OnUpdate();
        foreach (var buff in buffs)
        {
            buff.OnUpdate();
        }
        if(skillQueue.Count > 0 && !isSkillUsing)
        {
            isSkillUsing = true;
            skillQueue.Dequeue().Invoke();
            SkillDone();
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
        if(attack.buffInfos != null)
        {
            foreach(var buffInfo in attack.buffInfos)
            {
                GetBuff(buffInfo);
            }
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
        buff.SetBuff(buffInfo, this);
        buff.OnEnter();
        buffs.AddFirst(buff);
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
        if(NormalSkill != null)
            skillQueue.Enqueue(NormalSkill);
    }
    public void ActiveReinforcedSkill()
    {
        if(ReinforcedSkill != null)
            skillQueue.Enqueue(ReinforcedSkill);
    }
    public void ActiveSpecialSkill()
    {
        if(SpecialSkill != null)
            skillQueue.Enqueue(SpecialSkill);
    }

    public void SkillDone()
    {
        isSkillUsing = false;
    }
}
