using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEffectPool : MonoBehaviour
{
    [SerializeField]
    private EffectInfos[] effectInfos;

    private readonly Dictionary<EffectType, Queue<GameObject>> effectQueues = new();

    private void Awake()
    {
        for (int i = 0; i < effectInfos.Length; i++)
        {
            var parent = Instantiate(new GameObject(effectInfos[i].effectType.ToString()), transform);
            effectQueues.Add(effectInfos[i].effectType, new Queue<GameObject>());
            for (int j = 0; j < effectInfos[i].effectCount; j++)
            {
                var effect = Instantiate(effectInfos[i].effectPrefab, parent.transform);
                effect.SetActive(false);
                effectQueues[effectInfos[i].effectType].Enqueue(effect);
            }
        }
    }

    public GameObject GetEffect(EffectType effectType)
    {        
        GameObject effect = effectQueues[effectType].Dequeue();
        effect.SetActive(true);
        effectQueues[effectType].Enqueue(effect);
        return effect;
    }

    public void ReturnEffect(GameObject effect)
    {
        effect.SetActive(false);
        effectQueues[effect.GetComponent<Effects>().effectType].Enqueue(effect);        
    }

    public void Clear()
    {
        foreach (var effectQueue in effectQueues)
        {
            foreach (var effect in effectQueue.Value)
            {
                Destroy(effect);
            }
        }
    }

    private void OnDestroy()
    {
        Clear();
    }
}

[Serializable]
public class EffectInfos
{
    public EffectType effectType;
    public GameObject effectPrefab;
    public int effectCount;
}

public enum EffectType
{
    MeleeAttack,
    ProjectileAttack,
    String,
}