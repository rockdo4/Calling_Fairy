using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Creature : MonoBehaviour, IDamagable
{
    protected bool isLoaded = false;

    //public Image HpBackGround;
    //public Image HpBar;
    [HideInInspector]
    public GameObject HPBars;
    protected Slider HpBar;
    protected Slider ShieldBar;
    public Rigidbody2D Rigidbody { get; private set; }
    protected CreatureController CC;
    public Animator Animator { get; private set; }
    public List<Creature> targets = new();
    public float curHP { get; protected set; }
    public StageManager stageManager;

    public bool isAttacking = false;
    public bool isDead = false;
    public bool isKnockbacking = false;
    public bool isSkillUsing = false;

    protected Stack<SkillBase> skills = new();
    protected event Action NormalSkill;
    public int normalSkillID;
    protected event Action ReinforcedSkill;
    public int reinforcedSkillID;
    protected event Action SpecialSkill;
    public int SpecialSkillID;

    protected Queue<Action> skillQueue = new();

    public Vector2 CenterPos
    {
        get { return centerPos; }
    }
    protected Vector2 centerPos;

    [HideInInspector]
    public AttackType attackType;
    [HideInInspector]
    public GetTarget.TargettingType targettingType;
    [HideInInspector]
    public IAttackType attack;
    protected GetTarget getTarget;
    public LinkedList<BuffBase> activedBuffs = new();
    public LinkedList<BuffBase> awaitingBuffs = new();
    protected Stack<BuffBase> willRemoveBuffsList = new();
    public LinkedList<Shield> shields = new();
    public LinkedList<BuffBase> buffList()
    {
        if(activedBuffs!=null)
            return activedBuffs;
        
        return null;
    }
    
    public float skillCastTime = 0f;

    public DamageIndicator damageIndicator;

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
    public IngameStatus Realstatus
    {
        get { return realStatus; }
    }
    protected IngameStatus plusStatus;
    protected IngameStatus multipleStatus = new(IngameStatus.MakeType.Multiple);
    protected IngameStatus realStatus; //����
    protected IngameStatus returnStatus;
    protected String Type;
    public bool isDeadWithSkill = false;
    public readonly float DieSpeed = 1f;

    protected virtual void Awake()
    {
        var sliders = gameObject.GetComponentsInChildren<Slider>();
        foreach (var slider in sliders)
        {
            if (slider.CompareTag(Tags.HpBar))
            {
                HpBar = slider;
            }
            else if (slider.CompareTag(Tags.ShieldBar))
            {
                ShieldBar = slider;
            }
        }
        HPBars = HpBar.transform.parent.gameObject;
        HPBars.SetActive(true);
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        CC = new CreatureController(this);
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        gameObject.AddComponent<Knockback>();
        gameObject.AddComponent<Airborne>();
        gameObject.AddComponent<Die>();
        gameObject.AddComponent<Damaged>();
        gameObject.AddComponent<DamagedEffect>();
        gameObject.AddComponent<ExplosiveJump>();
        damageIndicator = gameObject.AddComponent<DamageIndicator>();
        getTarget = targettingType switch
        {
            GetTarget.TargettingType.AllInRange => new AllInRange(),
            GetTarget.TargettingType.SortingAtk => new SortingAtk(),
            GetTarget.TargettingType.SortingHp => new SortingHp(),
            _ => null
        };
        HpBar = GetComponentInChildren<Slider>();
        var col = GetComponentInChildren<Collider2D>();
        centerPos = col.bounds.center;
    }
    protected virtual void Start()
    {
        HpBar.maxValue = Status.hp;
        ShieldBar.maxValue = Status.hp;
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
    protected void Update()
    {
        CC.curState.OnUpdate();                
        if (awaitingBuffs.Count > 0)
        {
            HashSet<string> activeBuffNames = new(activedBuffs.Select(b => b.BuffInfo.buffName));
            List<BuffBase> buffWillApply = new();
            foreach (var awaitingBuff in awaitingBuffs)
            {
                if (!activeBuffNames.Contains(awaitingBuff.BuffInfo.buffName) ||
                    CheckBuffPriority(awaitingBuff,activedBuffs))
                {
                    buffWillApply.Add(awaitingBuff);
                }
            }
            foreach (var buff in buffWillApply)
            {
                ActiveBuff(buff);
            }
        }
        foreach (var buff in activedBuffs)
        {
            if (activedBuffs.Count == 0)
                break;
            buff.OnUpdate();
        }
        while (willRemoveBuffsList.Count > 0)
        {
            activedBuffs.Remove(willRemoveBuffsList.Pop());
        }
        if (skillQueue.Count > 0 && !isSkillUsing)
        {
            isSkillUsing = true;
            var skill = skillQueue.Dequeue();
            if(skill == NormalSkill)
            {
                Animator.SetTrigger("NormalSkill");
            }
            else if(skill == ReinforcedSkill)
            {
                Animator.SetTrigger("ReinforcedSkill");
            }
            else if(skill == SpecialSkill)
            {
                Animator.SetTrigger("SpecialSkill");
            }
        }
    }
    public void OnDamaged(AttackInfo attack)
    {
        if (UnityEngine.Random.value > attack.accuracy - Status.evasion)
        {
            damageIndicator.IndicateDamage(DamageType.Physical, 0, false, true);
            return;
        }
        isDeadWithSkill = attack.isSkill;
        var damagedStripts = GetComponents<IDamaged>();
        foreach (var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, attack);
        }        
        if(attack.buffInfo.buffName != null)
        {
            GetBuff(attack.buffInfo);
            //Debug.Log(attack.buffInfo.buffName);
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
    public void PlayAttackAnimation()
    {
        switch (attackType)
        {
            case AttackType.Melee:
                Animator.SetTrigger("MeleeAttack");
                break;
            case AttackType.Projectile:
                Animator.SetTrigger("ProjectileAttack");
                break;
            default:
                break;
        }
    }
    public void Attack()
    {
        attack.Attack();
    }    
    public void AttckFinished()
    {
        CC.ChangeState(StateController.State.Idle);
        StartCoroutine(AttackTimer());
    }
    private IEnumerator AttackTimer()
    {  
        if(!isAttacking)
            yield break;
        yield return new WaitForSeconds(1 / Status.attackSpeed);
        isAttacking = false;
    }
    private IEnumerator KnockbackTimer()
    {
        isKnockbacking = true;
        yield return new WaitForSeconds(0.5f);
        isKnockbacking = false;
    }
    public void Knockback(Vector2 vec)
    {
        if (isKnockbacking)
            return;
        StartCoroutine(KnockbackTimer());
        Rigidbody.AddForce(vec, ForceMode2D.Impulse);
    }
    public void GetBuff(in BuffInfo buffInfo)
    {
        var buff = BuffBase.MakeBuff(buffInfo);
        buff.SetCreature(this);
        awaitingBuffs.AddFirst(buff);
    }
    public void ActiveBuff(BuffBase buff)
    {
        awaitingBuffs.Remove(buff);
        buff.OnEnter();
        activedBuffs.AddFirst(buff);
    }
    public void RemoveBuff(BuffBase buff)
    {
        buff.OnExit();
        willRemoveBuffsList.Push(buff);
    }
    public bool CheckBuffPriority(BuffBase buff, in LinkedList<BuffBase> buffList)
    {        
        foreach (var buffInList in buffList)
        {            
            if (buff.BuffInfo.buffPriority > buffInList.BuffInfo.buffPriority)
            {
                RemoveBuff(buffInList);
                return true;
            }
        }
        return false;
    }
    public void LerpHpUI()
    {
        HpBar.value = curHP;
        ShieldBar.value = shields.Sum(s => s.leftshield);
    }
    public virtual void Die()
    {
        CC.ChangeState(StateController.State.Dead);
    }
    public void ActiveNormalSkill()
    {
        if(NormalSkill != null)
        {
            skillQueue.Enqueue(NormalSkill);
        }
    }
    public void ActiveReinforcedSkill()
    {
        if(ReinforcedSkill != null)
        {
            skillQueue.Enqueue(ReinforcedSkill);
        }
    }
    public void ActiveSpecialSkill()
    {
        if(SpecialSkill != null)
        {
            skillQueue.Enqueue(SpecialSkill);
        }
    }
    public void SkillDone()
    {
        isSkillUsing = false;
        CC.ChangeState(StateController.State.Idle);
    }
    public void Heal(float amount)
    {
        damageIndicator.IndicateDamage(DamageType.Magical, amount, false, false, true);
        curHP += amount;
        if (curHP > Status.hp)
            curHP = Status.hp;
    }
    public virtual void Damaged(float amount)
    {
        var temp = amount;
        if(shields.Count > 0)
        {            
            amount = shields.First.Value.DamagedShield(amount);            
        }
        curHP -= amount;
        //Debug.LogWarning($"{gameObject.name} damaged {temp} but {temp - amount} blocked {curHP} left");
        if (curHP <= 0)
        {
            curHP = 0;
            Die();
        }
    }
    public virtual void CastNormalSkill()
    {
        isAttacking = false;
        NormalSkill.Invoke();
    }
    public virtual void CastReinforcedSkill()
    {
        isAttacking = false;
        ReinforcedSkill.Invoke();
    }
    public virtual void CastSpecialSkill()
    {
        isAttacking = false;
        SpecialSkill.Invoke();
    }
}
