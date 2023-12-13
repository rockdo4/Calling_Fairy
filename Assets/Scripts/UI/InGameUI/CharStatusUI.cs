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

    public void GetCharStatusInfo(Creature cc)
    {
        CompareAttack(cc);
        ComparePhysicalArmor(cc);
        CompareMagicalArmor(cc);
        CompareAttackSpeed(cc);
    }

    private void CompareAttack(Creature cc)
    {
        if (Mathf.Approximately(cc.Status.damage, cc.Realstatus.damage))
        {
            if (hpUI.statusImages[statStatus.Normal].sprite == null)
                Debug.Log("널이다");
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.Normal].sprite;
            Debug.Log("1");
        }
        else if (cc.Status.attackFactor > cc.Realstatus.attackFactor)
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackUp].sprite;
            Debug.Log("2");
        }
        else
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackDown].sprite;
            Debug.Log("3");
        }
    }

    private void ComparePhysicalArmor(Creature cc)
    {
        if (Mathf.Approximately(cc.Status.physicalArmor, cc.Realstatus.physicalArmor))
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
        else if (cc.Status.physicalArmor > cc.Realstatus.physicalArmor)
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.PhysicalArmorUp].sprite;
        }
        else
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.PhysicalArmorDown].sprite;
        }
    }

    private void CompareMagicalArmor(Creature cc)
    {
        if (Mathf.Approximately(cc.Status.magicalArmor, cc.Realstatus.magicalArmor))
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
        else if (cc.Status.magicalArmor > cc.Realstatus.magicalArmor)
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.MagicalArmorUp].sprite;
        }
        else
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.MagicalArmorDown].sprite;
        }
    }

    private void CompareAttackSpeed(Creature cc)
    {
        if (Mathf.Approximately(cc.Status.attackSpeed, cc.Realstatus.attackSpeed))
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.Normal].sprite;
        }
        else if (cc.Status.attackSpeed > cc.Realstatus.attackSpeed)
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASUp].sprite;
        }
        else
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASDown].sprite;
        }
    }
}
