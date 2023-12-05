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

            //��� ����
            var attack = equipData.EquipPAttack == 0 ? equipData.EquipMAttack : equipData.EquipPAttack;
            equipStat.text = $"���ݷ� {attack, -10}�ִ� ü�� {equipData.EquipMaxHP}\n" +
                $"���� ���� {equipData.EquipPDefence, -10}���� ���� {equipData.EquipMDefence}";

            // equipIcon �̹��� ����


            //��� ����ġ ������
            var expTable = DataTableMgr.GetTable<EquipExpTable>();
            if (expTable.dic.TryGetValue(equipment.Level, out ExpData expData))
            {
                equipExpSlider.fillAmount = (float)equipment.Exp / expData.Exp;
            }
        }
    }

}
