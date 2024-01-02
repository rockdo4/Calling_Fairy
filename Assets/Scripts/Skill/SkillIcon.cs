using System.Collections.Generic;
using UnityEngine;

public class SkillIcon : PoolAble
{
    private SkillSpawn skillSpawn;
    //PanelDebug pD;
    [HideInInspector]
    public List<string> skillIconName = new List<string>();
    
    private void Awake()
    {
        skillSpawn = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();
        //pD = GameObject.FindWithTag(Tags.DebugMgr).GetComponent<PanelDebug>();
        GetSkillIcon(100001);
        GetSkillIcon(100002);
    }
    public void GetSkillIcon(int charID)
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[charID];
        var any = charData.toolTip.ToString();
        skillIconName.Add(any);

    }
    public List<string> SetSkillIcon()
    {
        return skillIconName;
    }

    public void SetReposition()
    {
        if (skillSpawn.stageCreatureInfo.IsStageEnd)
            return;
        skillSpawn.TouchSkill(gameObject);
        //ReleaseObject();
        //pD.GetBlockInfo();
    }
    private GameObject go;
    public void SetParticle()
    {
        go.GetComponent<SkillButtonEffect>().DieEffectOn();
    }
}