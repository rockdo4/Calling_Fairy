
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
        var charData = table.dic[fairyCard.ID];
        var fairyStat = fairyCard.FinalStat;
        realStatus = new IngameStatus(charData, fairyStat);
        attackType = charData.CharAttackType switch
        {
            1 => AttackType.Melee,
            2 => AttackType.Projectile,
            _ => AttackType.Count,
        };
        //targettingType = GetTarget.TargettingType.AllInRange;
        Status = realStatus;

        var skillTable = stageManager.thisIsSkillData;

        var normalSkillID = charData.CharSkill1;
        normalSkillData = skillTable.dic[normalSkillID];
        var normalSkill = SkillBase.MakeSkill(normalSkillData, this);
        skills.Push(normalSkill);
        NormalSkill += normalSkill.Active;

        var reinforcedSkillID = charData.CharSkill2;
        reinforceSkillData = skillTable.dic[reinforcedSkillID];
        var reinforceSkill = SkillBase.MakeSkill(reinforceSkillData, this);
        skills.Push(reinforceSkill);
        ReinforceSkill += reinforceSkill.Active;
        var fever = GameObject.FindWithTag(Tags.Fever).GetComponent<Fever>();
        feverEffect.Stop();
        fever.OnFeverStart += () => { feverEffect.Play(); };
        fever.OnFeverEnd += () => { feverEffect.Stop(); };

        normalAttackSE = Resources.Load<AudioClip>(charData.AttackSE);
        var skillID = stageManager.thisIsCharData.dic[charData.CharID].CharSkill1;
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
            script.SetPositionAndRotation(CenterPos + (Vector2)gameObject.transform.position, true);
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
