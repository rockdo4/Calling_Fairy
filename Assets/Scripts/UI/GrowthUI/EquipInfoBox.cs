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
        if (equipment == null)
        {
            equipName.text = "��� �̸�";

            //��� ����
            equipStat.text = $"���ݷ� {0,-20}�ִ� ü�� {0}\n���� ���� {0,-20}���� ���� {0}";

            // equipIcon �̹��� ����

            //��� ����ġ ������
            equipExpSlider.fillAmount = 0;
            return;
        }
            

        var table = DataTableMgr.GetTable<EquipTable>();
        if (table.dic.TryGetValue(equipment.ID, out EquipData equipData))
        {
            equipName.text = equipData.EquipName.ToString();

            var stat = StatCalculator(equipData, equipment.Level);
            //��� ����
            equipStat.text = $"���ݷ� {stat.attack, -20}�ִ� ü�� {stat.hp}\n" +
                $"���� ���� {stat.pDefence, -20}���� ���� {stat.mDefence}";

            // equipIcon �̹��� ����


            //��� ����ġ ������
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
            equipName.text = equipData.EquipName.ToString();

            var stat = StatCalculator(equipData, sampleLv);
            //��� ����
            equipStat.text = $"���ݷ� {stat.attack,-20}�ִ� ü�� {stat.hp}\n" +
                $"���� ���� {stat.pDefence,-20}���� ���� {stat.mDefence}";

            // equipIcon �̹��� ����


            //��� ����ġ ������
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

        result.attack = data.EquipAttack + data.EquipAttackIncrease * lv;
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * lv;
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * lv;
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * lv;

        return result;
    }

}
