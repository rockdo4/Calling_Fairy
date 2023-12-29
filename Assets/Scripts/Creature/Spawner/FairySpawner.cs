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
    private FormationSystem.Mode mode;
    
    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    public void SpawnCreatures()
    {
        var table = stageManager.thisIsCharData;
        var skilltable = stageManager.thisIsSkillData;

        if(GameManager.Instance.StageId >= 8001 && GameManager.Instance.StageId <= 8007)
        {
            mode = FormationSystem.Mode.Daily;
        }
        else if(GameManager.Instance.StageId >= 9001)
        {
            mode = FormationSystem.Mode.Story;
        }

        if(mode == FormationSystem.Mode.Story)
        {
            squard = GameManager.Instance.StoryFairySquad;
        }
        else if (mode == FormationSystem.Mode.Daily)
        {
            squard = GameManager.Instance.DailyFairySquad;
        }

        for (int i = 0; i < fairyNum; i++)
        {            
            var stat = table.dic[squard[i].ID];
            var fairyPrefab = Resources.Load<GameObject>(stat.CharAsset);
            var obj = Instantiate(fairyPrefab, gameObject.transform.position, Quaternion.identity);
            if(obj.TryGetComponent<Fairy>(out var fairyObject))
            {
                fairyObject.feverEffect = Instantiate(feverEffect, fairyObject.transform).GetComponent<ParticleSystem>();
                fairyObject?.SetData(squard[i]);
            }
            else
            {
                fairyObject = obj.AddComponent<Fairy>();
                fairyObject.feverEffect = Instantiate(feverEffect, fairyObject.transform).GetComponent<ParticleSystem>();
                fairyObject.SetData(squard[i]);
            }
            var ep = stageManager.effectPool;
            fairyObject.posNum = i;            
            var normalEffect = new EffectInfos
            {
                effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillNormal{i}"),
                effectPrefab = Resources.Load<GameObject>(skilltable.dic[stat.CharSkill1].skill_animation),
                effectCount = 10
            };
            ep.SetEffects(normalEffect);

            var reinforceEffect = new EffectInfos
            {
                effectType = (EffectType)Enum.Parse(typeof(EffectType), $"SkillReinforce{i}"),
                effectPrefab = Resources.Load<GameObject>(skilltable.dic[stat.CharSkill2].skill_animation),
                effectCount = 10
            };
            ep.SetEffects(reinforceEffect);
        }

    }
}
