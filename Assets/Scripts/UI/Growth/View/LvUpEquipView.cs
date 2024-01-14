using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvUpEquipView : GrowthView
{
    public TextMeshProUGUI equipName;
    public Image equipImage;
    public Image equipExpSlider;
    public Text equipExpText;
    public TextMeshProUGUI equipLvText;
    public TextMeshProUGUI equipAttackText;
    public TextMeshProUGUI equipHpText;
    public TextMeshProUGUI equipPDefenceText;
    public TextMeshProUGUI equipMDefenceText;
    public Transform enforceStoneSpace;

    public GameObject equipItemButtonPrefab;

    public override void UpdateUI()
    {
        ClearEnforceStoneScrollView();
        SetEnforceStoneScroolView();
        SetEquipGrowthInfoBox(DataTableMgr.GetTable<EquipTable>().dic[controller.SelectedSlot.Equipment.ID], controller.SelectedSlot.Equipment.Level, controller.SelectedSlot.Equipment.Exp);
    }

    public void SetEquipGrowthInfoBox(EquipData equipData, int lv, int exp)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        var expTable = DataTableMgr.GetTable<EquipExpTable>();

        equipLvText.text = $"{lv}";
        equipImage.sprite = Resources.Load<Sprite>(equipData.EquipIcon);
        equipName.text = stringTable.dic[equipData.EquipName].Value;
        equipExpSlider.fillAmount = (float)exp / expTable.dic[lv].Exp;
        equipExpText.text = $"{exp} / {expTable.dic[lv].Exp}";

        var stat = StatCalculator(equipData, lv);

        equipAttackText.text = stat.attack.ToString();
        equipHpText.text = stat.hp.ToString();
        equipPDefenceText.text = stat.pDefence.ToString();
        equipMDefenceText.text = stat.mDefence.ToString();
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

    public void SetEnforceStoneScroolView()
    {
        Set(10004);
        Set(10005);
        Set(10006);

        void Set(int id)
        {
            if (InvManager.itemInv.Inven.TryGetValue(id, out Item enforceStone))
            {
                if (enforceStone.Count > 0)
                {
                    var go = Instantiate(equipItemButtonPrefab, enforceStoneSpace);
                    var itemButton = go.GetComponent<ItemButton>();
                    
                    itemButton.Init(enforceStone);
                    itemButton.OnAddButtonClick += controller.SelectEquipExpItem;
                    itemButton.OnSubtractButtonClick += controller.DeselectEquipExpItem;
                }
            }
        }
    }

    public void ClearEnforceStoneScrollView()
    {
        for (int i = enforceStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = enforceStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
        //enforceStoneButtons.Clear();
    }

}
