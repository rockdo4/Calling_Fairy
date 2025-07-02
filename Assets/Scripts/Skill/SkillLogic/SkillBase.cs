using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    private int ID;
    protected SkillData skillData;
    protected Creature owner;
    protected AttackInfo[] attackInfos;
    protected List<Creature>[] targets = new List<Creature>[3];
    private int layerMask;

    private SkillGroup skillGroup;
    //targets0 - Enemy, targets1 - Ally, targets2 - Self
    public static SkillBase MakeSkill(in SkillData skillData, Creature creature)
    {
        SkillBase rtn = skillData.skill_projectileID switch
        {
            0 => new MeleeSkill(),
            _ => new ProjectileSkill(),
        };
        rtn.SetData(skillData, creature);
        return rtn;
    }

    private void SetData(in SkillData skillData, Creature creature)
    {        
        owner = creature;
        layerMask = LayerMask.GetMask(Layers.Player,Layers.Monster);
        this.skillData = skillData;
        ID = skillData.skill_ID;
        skillGroup = (SkillGroup)skillData.skill_group;
        attackInfos = new AttackInfo[skillData.skill_detail.Count];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = new List<Creature>();
        }
        targets[(int)TargetingType.Self].Add(owner);

        for(int i = 0; i < attackInfos.Length; i++)
        {     
            attackInfos[i].attacker = creature.gameObject;
            attackInfos[i].accuracy = float.MaxValue;
            attackInfos[i].targetingType = skillData.skill_detail[i].skill_appType;
            attackInfos[i].isSkill = true;
            var fairy = creature as Fairy;
            var skillTypeName = skillGroup switch
            { 
                SkillGroup.Chain2 => "SkillNormalTarget",
                SkillGroup.Chain3 => "SkillReinforceTarget",
                _ => null,
            };
            //test
            if(ID is 20005 or 20006)
            {
                Debug.Log($"{skillTypeName}{fairy?.posNum}_{i - 1}");
            }
            //test
            if(i != 0 && fairy != null && skillTypeName != null)
            {                
                attackInfos[i].effectType = (EffectType)Enum.Parse(typeof(EffectType), $"{skillTypeName}{fairy.posNum}_{i - 1}");
            }
            if (skillData.skill_detail[i].skill_practiceType == 1)
            {
                if (skillData.skill_detail[i].skill_numType == SkillNumType.Int)
                {
                    attackInfos[i].damage = skillData.skill_detail[i].skill_multipleValue;
                }
                else
                {
                    attackInfos[i].damage = creature.Status.damage * skillData.skill_detail[i].skill_multipleValue / 100;
                }
                if(i == 0)
                {
                    attackInfos[i].knockbackDistance = skillData.skill_kbValue;
                    attackInfos[i].airborneDistance = skillData.skill_abValue;                
                }
                else
                {
                    attackInfos[i].airborneDistance = 0;
                    attackInfos[i].knockbackDistance = 0;
                }
                attackInfos[i].targetingType = skillData.skill_detail[i].skill_appType;
            }
            else
            {
                attackInfos[i].buffInfo = new BuffInfo
                {
                    buffName = skillData.skill_name,
                    duration = skillData.skill_detail[i].skill_time,
                    value = skillData.skill_detail[i].skill_multipleValue,
                    buffType = (BuffType)skillData.skill_detail[i].skill_practiceType,
                    isPercent = skillData.skill_detail[i].skill_numType == SkillNumType.Percent,
                    buffPriority = skillData.skill_group,                    
                };                
            }
        }
    }
    public virtual void Active()
    {
        //Debug.Log($"{ID}");
        owner.skillAttackSE.PlaySe();
        GetTargets();
        foreach(var attackInfo in attackInfos) 
        {
            var tgts = targets[(int)attackInfo.targetingType];
            foreach(var tgt in tgts)
            {      
                if(!tgt)
                    continue;

                tgt.OnDamaged(attackInfo);
            }
        }
    }

    protected void GetTargets()
    {
        targets[(int)TargetingType.Ally].Clear();
        targets[(int)TargetingType.Enemy].Clear();
        var pos = owner.transform.position;        
        var inRangecreatures = Physics2D.OverlapCircleAll(pos, skillData.skill_range, layerMask);
        foreach(var tgt in inRangecreatures)
        {
            var script = tgt.GetComponent<Creature>();
            if(!script || script.isDead)
                continue;

            var targetType = tgt.CompareTag(owner.tag) ? TargetingType.Ally : TargetingType.Enemy;
            targets[(int)targetType].Add(script);
        }
    }
}