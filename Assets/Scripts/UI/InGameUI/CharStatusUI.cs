using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharStatusUI : MonoBehaviour
{
    private HPUI hpUI;
    [Header("캐릭터 상태 정보 칸")]
    public Image[] charStatusInfo = new Image[4];
    private LinkedList<BuffBase> buffLinkList = new LinkedList<BuffBase>();
    private bool buffCheck = false;
    private void Awake()
    {
        hpUI = GetComponentInParent<HPUI>();
    }

    public void GetCharStatusInfo(Creature cc)
    {
        buffLinkList = cc.buffList();
        CompareAttack(cc);
        ComparePhysicalArmor(cc);
        CompareMagicalArmor(cc);
        CompareAttackSpeed(cc);
    }

    private void CompareAttack(Creature cc)
    {
        buffCheck = false;
        foreach (var c in buffLinkList)
        {
            if (c.BuffInfo.buffType == BuffType.AtkDmgBuff)
            {
                buffCheck = true;
                break;
            }
        }
        if (!buffCheck)
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.Normal];
            return;
        }


        if (Mathf.Approximately(cc.Status.damage, cc.Realstatus.damage))
        {
            if (hpUI.statusImages[statStatus.Normal] == null)
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackNormal];
        }
        else if (cc.Status.damage > cc.Realstatus.damage)
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackUp];
        }
        else
        {
            charStatusInfo[0].sprite = hpUI.statusImages[statStatus.AttackDown];
        }
    }

    private void ComparePhysicalArmor(Creature cc)
    {
        buffCheck = false;
        foreach (var c in buffLinkList)
        {
            if (c.BuffInfo.buffType == BuffType.PDefBuff)
            {
                buffCheck = true;
                break;
            }
        }
        if (!buffCheck)
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.Normal];
            return;
        }


        if (Mathf.Approximately(cc.Status.physicalArmor, cc.Realstatus.physicalArmor))
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.PhysicalArmorNormal];
        }
        else if (cc.Status.physicalArmor > cc.Realstatus.physicalArmor)
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.PhysicalArmorUp];
        }
        else
        {
            charStatusInfo[1].sprite = hpUI.statusImages[statStatus.PhysicalArmorDown];
        }
    }

    private void CompareMagicalArmor(Creature cc)
    {
        buffCheck = false;
        foreach (var c in buffLinkList)
        {
            if (c.BuffInfo.buffType == BuffType.MDefBuff)
            {
                buffCheck = true;
                break;
            }
        }
        if (!buffCheck)
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.Normal];
            return;
        }


        if (Mathf.Approximately(cc.Status.magicalArmor, cc.Realstatus.magicalArmor))
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.MagicalArmorNormal];
        }
        else if (cc.Status.magicalArmor > cc.Realstatus.magicalArmor)
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.MagicalArmorUp];
        }
        else
        {
            charStatusInfo[2].sprite = hpUI.statusImages[statStatus.MagicalArmorDown];
        }
    }

    private void CompareAttackSpeed(Creature cc)
    {
        buffCheck = false;
        foreach (var c in buffLinkList)
        {
            if (c.BuffInfo.buffType == BuffType.AtkSpdBuff)
            {
                buffCheck = true;
                break;
            }
        }
        if (!buffCheck)
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.Normal];
            return;
        }


        if (Mathf.Approximately(cc.Status.attackSpeed, cc.Realstatus.attackSpeed))
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASNormal];
        }
        else if (cc.Status.attackSpeed > cc.Realstatus.attackSpeed)
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASUp];
        }
        else
        {
            charStatusInfo[3].sprite = hpUI.statusImages[statStatus.ASDown];
        }
    }
}
