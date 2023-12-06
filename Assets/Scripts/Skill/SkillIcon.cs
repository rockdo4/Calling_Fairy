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
        GetSkillICon(100001);
    }
    private void GetSkillICon(int charID)
    {
        var charTable = DataTableMgr.GetTable<CharacterTable>();
        var skillInfo = charTable.dic[charID].CharSkill;
        var skillIconInfo = DataTableMgr.GetTable<SkillTable>();
        var skillIcon = skillIconInfo.dic[skillInfo].skill_detail[skillInfo].skill_multipleValue;
        Debug.Log(skillIcon);

    }
    public void SetReposition()
    {
        skillSpawn.TouchSkill(gameObject);
        //ReleaseObject();
        pD.GetBlockInfo();
    }

}