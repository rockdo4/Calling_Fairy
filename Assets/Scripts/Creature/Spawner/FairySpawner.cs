using System;
using UnityEngine;

public class FairySpawner : MonoBehaviour
{
    [SerializeField]
    private int fairyNum;
    protected StageManager stageManager;
    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    public void SpawnCreatures()
    {
        var table = stageManager.thisIsCharData;
        var skilltable = stageManager.thisIsSkillData;
        for (int i = 0; i < fairyNum; i++)
        {
            var stat = table.dic[GameManager.Instance.StoryFairySquad[i].ID];
            var fairyPrefab = Resources.Load<GameObject>(stat.CharAsset);
            var obj = Instantiate(fairyPrefab, gameObject.transform.position, Quaternion.identity);
            if(obj.TryGetComponent<Fairy>(out var fairyObject))
            {
                fairyObject?.SetData(GameManager.Instance.StoryFairySquad[i]);
            }
            else
            {
                fairyObject = obj.AddComponent<Fairy>();
                fairyObject.SetData(GameManager.Instance.StoryFairySquad[i]);
            }
            fairyObject.posNum = i;            
            var ep = stageManager.effectPool;
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
