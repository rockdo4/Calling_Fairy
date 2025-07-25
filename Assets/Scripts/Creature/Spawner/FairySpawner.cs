using System;
using UnityEngine;

public class FairySpawner : MonoBehaviour
{
    [SerializeField]
    private int fairyNum;
    [SerializeField]
    private GameObject feverEffect;
    protected StageManager stageManager;
    private FairyCard[] squard;
    
    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    public void SpawnCreatures()
    {
        var table = stageManager.thisIsCharData;
        var skilltable = stageManager.thisIsSkillData;
        var stageMode = (Mode)stageManager.thisIsStageData.dic[GameManager.Instance.StageId].stagetype;

        if (stageMode == Mode.Story)
        {
            squard = GameManager.Instance.StoryFairySquad;
        }
        else if (stageMode == Mode.Daily)
        {
            squard = GameManager.Instance.DailyFairySquad;
        }

        for (int i = 0; i < fairyNum; i++)
        {
            var stat = table.dic[squard[i].ID];
            var fairyPrefab = Resources.Load<GameObject>(stat.CharAsset);
            var obj = Instantiate(fairyPrefab, gameObject.transform.position, Quaternion.identity);
            if (obj.TryGetComponent<Fairy>(out var fairyObject))
            {
                fairyObject.posNum = i;
                fairyObject.feverEffect = Instantiate(feverEffect, fairyObject.transform).GetComponent<ParticleSystem>();
                fairyObject?.SetData(squard[i]);
            }
            else
            {
                fairyObject = obj.AddComponent<Fairy>();
                fairyObject.posNum = i;
                fairyObject.feverEffect = Instantiate(feverEffect, fairyObject.transform).GetComponent<ParticleSystem>();
                fairyObject.SetData(squard[i]);
            }
            var ep = stageManager.effectPool;
            var normalEffect = new EffectInfos
            {
                effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillNormal{i}"),
                effectPrefab = Resources.Load<GameObject>(skilltable.dic[stat.CharSkill1].skill_animation),
                effectCount = 10

            };
            ep.SetEffects(normalEffect);
            for (int j = 0; j < fairyObject.normalSkillData.skill_detail.Count; j++)
            {
                var skillEffectPath = fairyObject.normalSkillData.skill_detail[j].skill_appAnimation;
                if (skillEffectPath == null || skillEffectPath == "0" || j == 0)
                    continue;
                var normalEffectTarget = new EffectInfos
                {
                    effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillNormalTarget{i}_{j - 1}"),
                    effectPrefab = Resources.Load<GameObject>(skillEffectPath),
                    effectCount = 10
                };
                ep.SetEffects(normalEffectTarget);
            }
            var reinforceEffect = new EffectInfos
            {
                effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillReinforce{i}"),
                effectPrefab = Resources.Load<GameObject>(skilltable.dic[stat.CharSkill2].skill_animation),
                effectCount = 10
            };
            ep.SetEffects(reinforceEffect);
            for (int j = 0; j < fairyObject.reinforceSkillData.skill_detail.Count; j++)
            {
                var skillEffectPath = fairyObject.reinforceSkillData.skill_detail[j].skill_appAnimation;
                if (skillEffectPath == null || skillEffectPath == "0" || j == 0)
                    continue;
                var reinforceEffectTarget = new EffectInfos
                {
                    effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillReinforceTarget{i}_{j - 1}"),
                    effectPrefab = Resources.Load<GameObject>(skillEffectPath),
                    effectCount = 10
                };
                ep.SetEffects(reinforceEffectTarget);
            }
        }
    }
}
