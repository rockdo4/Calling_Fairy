using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
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
    public float curHP { get; protected set; }
    public StageManager stageManager;
    public bool isAttacking = false;
    public bool isDead = false;
    public bool isKnockbacking = false;

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
    public LinkedList<BuffBase> activedBuffs = new();
    public LinkedList<BuffBase> awaitingBuffs = new();
    protected Stack<BuffBase> willRemoveBuffsList = new();
    public LinkedList<Shield> shields = new();

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
            skillQueue.Dequeue().Invoke();
            SkillDone();
        }
    }

    public void OnDamaged(in AttackInfo attack)
    {
        if (UnityEngine.Random.value > attack.accuracy - Status.evasion)        
             return;
        
        var damagedStripts = GetComponents<IDamaged>();
        foreach (var damagedStript in damagedStripts)
        {
            damagedStript.OnDamage(gameObject, attack);
        }        
        if(attack.buffInfo.buffName != null)
        {
            GetBuff(attack.buffInfo);
            Debug.Log(attack.buffInfo.buffName);
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

    public void Attack()
    {
        if (isAttacking)
            return;
        StartCoroutine(AttackTimer());
        attack.Attack();
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
        HpBar.value = curHP / Status.hp;
    }

    public virtual void Die()
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

    public void Heal(float amount)
    {
        curHP += amount;
        if (curHP > Status.hp)
            curHP = Status.hp;
    }
    public void Damaged(float amount)
    {
        var temp = amount;
        if(shields.Count > 0)
        {            
            amount = shields.First.Value.DamagedShield(amount);            
        }
        curHP -= amount;
        Debug.LogWarning($"{gameObject.name} damaged {temp} but {temp - amount} blocked");
        if (curHP <= 0)
        {
            curHP = 0;
            Die();
        }
    }
}
