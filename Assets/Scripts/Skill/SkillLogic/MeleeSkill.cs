using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkill : SkillBase
{
    public override void Active()
    {
        Debug.Log("MeleeSkill Active");
        base.Active();
    }
}
