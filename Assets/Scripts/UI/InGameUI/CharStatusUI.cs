using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharStatusUI : MonoBehaviour
{
    HPUI hpUI;
    [Header("캐릭터 상태 정보 칸")]
    public Image[] charStatusInfo = new Image[4];
    private void Awake()
    {
        hpUI = GetComponentInParent<HPUI>();
    }
    private void Update()
    {
        GetCharStatusInfo();
    }

    private void GetCharStatusInfo()
    {
        //공격력 비교.
        CompareAttack();
        CompareDefence();
        CompareHeal();
        CompareAttackSpeed();
    }

    private void CompareAttack()
    {
        //if()
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackUp].sprite;
        }
        //else if (1==2)
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackDown].sprite;
        }
        //else
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
    }

    private void CompareDefence()
    {
        //if()
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.DefenceUp].sprite;
        }
        //else if (1==2)
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.DefenceDown].sprite;
        }
        //else
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
    }

    private void CompareHeal()
    {
        //if()
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.HealUp].sprite;
        }
        //else if (1==2)
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.HealDown].sprite;
        }
        //else
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
    }

    private void CompareAttackSpeed()
    {
        //if()
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASUp].sprite;
        }
        //else if (1==2)
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASDown].sprite;
        }
        //else
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
    }
}
