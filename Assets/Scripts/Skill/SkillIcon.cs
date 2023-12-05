using System;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SkillIcon : PoolAble
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
        //ReleaseObject();
        pD.GetBlockInfo();
    }

}