using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    protected int ID;
    protected SkillData skillData;
    protected Creature creature;
    protected GetTarget getTarget;
    protected AttackInfo[] attackInfos;
    protected List<Creature>[] targets = new List<Creature>[3];

    public SkillGroup skillGroup;
    //targets0 - Self, targets1 - Ally, targets2 - Enemy
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
    public virtual void SetData(in SkillData skillData, Creature creature)
    {        
        this.creature = creature;
        skillGroup = (SkillGroup)skillData.skill_group;
        attackInfos = new AttackInfo[skillData.skill_detail.Count];
        for(int i = 0; i < attackInfos.Length; i++)
        {
            var atk = attackInfos[i];
            if (skillData.skill_detail[i].skill_numType == SkillNumType.Int)
            {
                atk.damage = skillData.skill_detail[i].skill_multipleValue;
            }
            else
            {
                atk.damage = creature.Status.damage * skillData.skill_detail[i].skill_multipleValue;
            }
            atk.attacker = creature.gameObject;
            atk.accuracy = float.MaxValue;
            atk.targetingType = skillData.skill_detail[i].skill_appType;
            if(i == 0)
            {
                atk.knockbackDistance = skillData.skill_kbValue;
                atk.airborneDistance = skillData.skill_abValue;                
            }
            else
            {
                atk.airborneDistance = 0;
                atk.knockbackDistance = 0;
            }
            atk.targetingType = skillData.skill_detail[i].skill_appType;
        }
    }
    public virtual void Active()
    {
        GetTargets();
        foreach(var attackInfo in attackInfos) 
        {
            var tgts = attackInfo.targetingType switch
            {
                TargetingType.Self => targets[0],
                TargetingType.Ally => targets[1],
                TargetingType.Enemy => targets[2],
                _ => null,
            };
            foreach(var tgt in tgts)
            {
                tgt.OnDamaged(attackInfo);
            }
        }
    }

    protected virtual void GetTargets()
    {
        foreach(var list in targets)
        {
            list.Clear();
        }
        targets[0].Add(creature);
        targets[1].Add(creature);
        LayerMask layerMask = LayerMask.GetMask(Layers.Player,Layers.Monster);
        var pos = creature.transform.position;        
        var inRangecreatures = Physics2D.OverlapCircleAll(pos, skillData.skill_range, layerMask);
        foreach(var tgt in inRangecreatures)
        {
            var script = creature.GetComponent<Creature>();
            if(tgt.CompareTag(creature.tag))
            {
                targets[1].Add(script);
            }
            else
            {
                targets[2].Add(script);
            }
        }
    }
}