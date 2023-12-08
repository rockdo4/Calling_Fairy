using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{

    private bool isAutoPlay = false;
    private SkillSpawn skillSpawn;
    [SerializeField]
    private float autoTimer = 0.5f;
    private float addTime = 0;
    private void Awake()
    {
        skillSpawn = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();
    }
    public void SetAutoPlay(bool isAuto)
    {
        isAutoPlay = isAuto;
    }
    private void Update()
    {
        addTime += Time.deltaTime;
        if (isAutoPlay)
        {
            if (skillSpawn.skillWaitList.Count <= 0 || skillSpawn == null)
                return;
            if (autoTimer < addTime && Mathf.Approximately(skillSpawn.skillWaitList[0].SkillObject.transform.position.x, skillSpawn.GetSkillPos(0).x))
            {
                if (skillSpawn.skillWaitList.Count > 0)
                {
                    skillSpawn.TouchSkill(skillSpawn.skillWaitList[0].SkillObject);
                    addTime = 0f;
                }
            }
        }
    }

}
