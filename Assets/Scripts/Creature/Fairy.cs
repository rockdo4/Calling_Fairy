
using System;
using System.Net.Http.Headers;
using UnityEngine;
public class Fairy : Creature
{
    private HPUI hpUI;
    public int posNum;
    public ParticleSystem feverEffect;

    protected override void Start()
    {
        stageManager.playerParty.Add(this);
        hpUI = GameObject.FindWithTag(Tags.HPUI).GetComponent<HPUI>();
        base.Start();
    }
    public void SetData(FairyCard fairyCard)
    {
        gameObject.tag = Tags.Player;
        gameObject.layer = LayerMask.NameToLayer(Layers.Player);
        isLoaded = true;
        var table = DataTableMgr.GetTable<CharacterTable>();
        var stat = table.dic[fairyCard.ID];
        var fairyStat = fairyCard.FinalStat;
        realStatus.hp = fairyStat.hp;
        realStatus.damage = fairyStat.attack;
        realStatus.damageType = DamageType.Physical;
        realStatus.physicalArmor = fairyStat.pDefence;
        realStatus.magicalArmor = fairyStat.mDefence;
        realStatus.criticalChance = fairyStat.criticalRate;
        realStatus.criticalFactor = stat.CharCritFactor;
        realStatus.evasion = fairyStat.avoid;
        realStatus.accuracy = fairyStat.accuracy;
        realStatus.attackSpeed = fairyStat.attackSpeed;
        realStatus.attackRange = stat.CharAttackRange;
        realStatus.basicMoveSpeed = stat.CharMoveSpeed;
        realStatus.moveSpeed = 100f;
        realStatus.knockbackDistance = stat.CharKnockback;
        realStatus.knockbackResist = fairyStat.resistance;
        realStatus.attackFactor = stat.CharAttackFactor;
        realStatus.projectileDuration = stat.CharAttackProjectile;
        realStatus.projectileHeight = stat.CharAttackHeight;
        attackType = stat.CharAttackType switch
        {
            1 => AttackType.Melee,
            2 => AttackType.Projectile,
            _ => AttackType.Count,
        };
        targettingType = GetTarget.TargettingType.AllInRange;
        returnStatus = realStatus;

        var skillTable = stageManager.thisIsSkillData;
        var effectPool = stageManager.effectPool;
        var effect = new EffectInfos();

        var normalSkillID = stat.CharSkill1;
        normalSkillData = skillTable.dic[normalSkillID];
        var normalSkill = SkillBase.MakeSkill(normalSkillData, this);
        skills.Push(normalSkill);
        NormalSkill += normalSkill.Active;

        var reinforcedSkillID = stat.CharSkill2;
        reinforceSkillData = skillTable.dic[reinforcedSkillID];
        var reinforceSkill = SkillBase.MakeSkill(reinforceSkillData, this);
        skills.Push(reinforceSkill);
        ReinforceSkill += reinforceSkill.Active;
        var fever = GameObject.FindWithTag(Tags.Fever).GetComponent<Fever>();
        feverEffect.Stop();
        fever.OnFeverStart += () => { feverEffect.Play(); };
        fever.OnFeverEnd += () => { feverEffect.Stop(); };

        normalAttackSE = Resources.Load<AudioClip>(stat.AttackSE);
        var skillID = stageManager.thisIsCharData.dic[stat.CharID].CharSkill1;
        skillAttackSE = Resources.Load<AudioClip>(skillTable.dic[skillID].SkillSE);
    }
    public override void Damaged(float amount)
    {
        base.Damaged(amount);
        hpUI.HPUIUpdate();
        hpUI.SetStatus();
    }

    public override void CastNormalSkill()
    {
        var eft = stageManager.effectPool.GetEffect((EffectType)Enum.Parse(typeof(EffectType),$"SkillNormal{posNum}"));
        if (eft == null)
            return;
        if (!eft.TryGetComponent<SkillEffects>(out var script))
            return;
        if (stageManager.thisIsSkillData.dic[normalSkillData.skill_ID].skill_motionFollow == 1)
        {
            script.SetPositionAndRotation(CenterPos + (Vector2)gameObject.transform.position, true, new Vector3(), gameObject);
        }
        else
        {
            script.SetPositionAndRotation(CenterPos + (Vector2)gameObject.transform.position, true, new Vector3());
        }
        base.CastNormalSkill();
    }
    public override void CastReinforcedSkill()
    {
        var eft = stageManager.effectPool.GetEffect((EffectType)Enum.Parse(typeof(EffectType), $"SkillReinforce{posNum}"));
        if (eft == null)
            return;
        if(!eft.TryGetComponent<SkillEffects>(out var script))
            return; 
        if (stageManager.thisIsSkillData.dic[reinforceSkillData.skill_ID].skill_motionFollow == 1)
        {
            script.SetPositionAndRotation(CenterPos + (Vector2)gameObject.transform.position, true, new Vector3(), gameObject);
        }
        else
        {
            script.SetPositionAndRotation(CenterPos + (Vector2)gameObject.transform.position, true, new Vector3());
        }
        base.CastReinforcedSkill();
    }
    public override void CastSpecialSkill()
    {
        base.CastSpecialSkill();
    }
}
