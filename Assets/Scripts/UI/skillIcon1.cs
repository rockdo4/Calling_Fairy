using System.Collections.Generic;
using UnityEngine;

public class SkillIcon1 : PoolAble
{
    NewBehaviourScript newBehaviourScript;
    //PanelDebug pD;
    public List<string> skillIconName = new List<string>();
    private void Awake()
    {
        newBehaviourScript = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<NewBehaviourScript>();
        //pD = GameObject.FindWithTag(Tags.DebugMgr).GetComponent<PanelDebug>();
        //GetSkillIcon(100001);
        //GetSkillIcon(100002);
    }
    //public void GetSkillIcon(int charID)
    //{
    //    var charData = DataTableMgr.GetTable<CharacterTable>().dic[charID];
    //    var any = charData.toolTip.ToString();
    //    skillIconName.Add(any);

    //}
    //public List<string> SetSkillIcon()
    //{
    //    //return skillIconName;
    //}

    public void SetReposition()
    {
        newBehaviourScript.TouchSkill(gameObject);
        //ReleaseObject();
        //pD.GetBlockInfo();
    }

}