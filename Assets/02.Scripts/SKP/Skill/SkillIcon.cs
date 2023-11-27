using System;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    
    public void SetReposition()
    {
        SkillNow();

        SkillSpawn.Instance.TouchSkill(gameObject); 
    }

    private void SkillNow()
    {
        for (int i = 0; i < PlayerChecker.Instance.fairyDieCheck.Length; i++)
        {
            if (PlayerChecker.Instance.fairyDieCheck[i])
            {

            }
        }
    }
}