using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FairyGrowthUI;

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
        if (equipment == null)
            return;

        var table = DataTableMgr.GetTable<EquipTable>();
        if (table.dic.TryGetValue(equipment.ID, out EquipData equipData))
        {
            equipName.text = equipData.EquipName;

            //장비 스탯
            var attack = equipData.EquipPAttack == 0 ? equipData.EquipMAttack : equipData.EquipPAttack;
            equipStat.text = $"공격력 {attack, -20}최대 체력 {equipData.EquipMaxHP}\n" +
                $"물리 방어력 {equipData.EquipPDefence, -20}마법 방어력 {equipData.EquipMDefence}";

            // equipIcon 이미지 변경


            //장비 경험치 게이지
            var expTable = DataTableMgr.GetTable<EquipExpTable>();
            if (expTable.dic.TryGetValue(equipment.Level, out ExpData expData))
            {
                equipExpSlider.fillAmount = (float)equipment.Exp / expData.Exp;
            }
        }
    }

    public void SetEquipInfo(Equipment equipment, int sampleLv, int sampleExp)
    {
        var table = DataTableMgr.GetTable<EquipTable>();
        if (table.dic.TryGetValue(equipment.ID, out EquipData equipData))
        {
            equipName.text = equipData.EquipName;

            var stat = StatCalculator(equipData, sampleLv);
            //장비 스탯
            equipStat.text = $"공격력 {stat.attack,-20}최대 체력 {stat.hp}\n" +
                $"물리 방어력 {stat.pDefence,-20}마법 방어력 {stat.mDefence}";

            // equipIcon 이미지 변경


            //장비 경험치 게이지
            var expTable = DataTableMgr.GetTable<EquipExpTable>();
            if (expTable.dic.TryGetValue(sampleLv, out ExpData expData))
            {
                equipExpSlider.fillAmount = (float)sampleExp / expData.Exp;
            }
        }
    }
    public Stat StatCalculator(EquipData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.EquipPAttack + data.EquipPAttackIncrease * lv;
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * lv;
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * lv;
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * lv;

        return result;
    }

}
