using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    SkillSpawn skillSpawn;
    PanelDebug pD;
    
    private void Awake()
    {
        skillSpawn = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();
        pD = GameObject.FindWithTag(Tags.DebugMgr).GetComponent<PanelDebug>();
    }

    public void SetReposition()
    {
        skillSpawn.TouchSkill(gameObject);
        pD.GetBlockInfo();
    }

    
}