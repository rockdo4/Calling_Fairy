using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static IAttackType;

public class SkillBase
{
    protected int ID;
    protected SkillData skillData;
    protected Creature creature;
    protected GetTarget getTarget;
    protected AttackInfo attackInfo;
    protected SkillEffect[] skillEffects;
    protected List<Creature> targets = new();    

    public static SkillBase MakeSkill(SkillData skillData, Creature creature)
    {        
        SkillBase rtn = new();
        rtn.SetData(skillData, creature);
        return rtn;
    }
    public virtual void SetData(SkillData skillData, Creature creature)
    {        
        this.creature = creature;
        foreach(var skillInfo in skillData.skill_detail)
        {
            switch(skillInfo.skill_appType)
            {
                case 0:                    
                    break;
                case 1:
                    
                    break;
                case 2:
                    targets.Add(creature);
                    break;
                    default:
                    break;
            }
        }
    }
    public virtual void Active()
    {
        targets.Clear();
    }

    protected void GetTargets(Creature target)
    {
        targets.Add(target);
    }

    public class SkillEffect
    {

    }    
}