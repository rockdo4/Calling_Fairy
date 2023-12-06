using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoBox : MonoBehaviour, IUIElement
{
    public TextMeshProUGUI equipName;
    public TextMeshProUGUI equipStat;
    public Image equipIcon;
    public Image equipExpSlider;
    public Button detailsInfoButton;

    public void Init(Card card)
    {
    }

    public void SetEquipInfo(Equipment equipment)
    {
        var table = DataTableMgr.GetTable<EquipTable>();
        if (table.dic.TryGetValue(equipment.ID, out EquipData equipData))
        {
            equipName.text = equipData.EquipName;

            //장비 스탯
            var attack = equipData.EquipPAttack == 0 ? equipData.EquipMAttack : equipData.EquipPAttack;
            equipStat.text = $"공격력 {attack, -10}최대 체력 {equipData.EquipMaxHP}\n" +
                $"물리 방어력 {equipData.EquipPDefence, -10}마법 방어력 {equipData.EquipMDefence}";

            // equipIcon 이미지 변경


            //장비 경험치 게이지
            var expTable = DataTableMgr.GetTable<EquipExpTable>();
            if (expTable.dic.TryGetValue(equipment.Level, out ExpData expData))
            {
                equipExpSlider.fillAmount = (float)equipment.Exp / expData.Exp;
            }
        }
    }

}
