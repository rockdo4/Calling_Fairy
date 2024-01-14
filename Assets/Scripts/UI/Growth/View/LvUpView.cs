using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvUpView : GrowthView
{

    public TextMeshProUGUI lvText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pDefenceText;
    public TextMeshProUGUI mDefenceText;

    public TextMeshProUGUI expText;
    public Slider expSlider;

    public GameObject itemButtonPrefab;
    public Transform spiritStoneSpace;

    public override void UpdateUI()
    {
        ClearSpiritStoneScrollView();
        SetSpiritStoneScroolView();
        UpdateStatText(controller.SelectFairy.Level, controller.SelectFairy.Experience);
    }

    public void UpdateStatText(int level, int exp)
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[controller.SelectFairy.ID];
        var expTable = DataTableMgr.GetTable<ExpTable>();

        var stat = StatCalculator(charData, level);
        lvText.text = level.ToString();
        attackText.text = stat.attack.ToString();
        hpText.text = stat.hp.ToString();
        pDefenceText.text = stat.pDefence.ToString();
        mDefenceText.text = stat.mDefence.ToString();
        expText.text = $"{exp} / {expTable.dic[level].Exp}";
        expSlider.value = (float)exp / expTable.dic[level].Exp;
    }

    public Stat StatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.CharAttack + data.CharAttackIncrease * lv - 1;
        result.pDefence = data.CharPDefence + data.CharPDefenceIncrease * lv - 1;
        result.mDefence = data.CharMDefence + data.CharMDefenceIncrease * lv - 1;
        result.hp = data.CharMaxHP + data.CharHPIncrease * lv - 1;

        return result;
    }

    public void SetSpiritStoneScroolView()
    {
        foreach (var dir in InvManager.spiritStoneInv.Inven)
        {
            if (dir.Value.Count == 0)
            {
                continue;
            }

            var go = Instantiate(itemButtonPrefab, spiritStoneSpace);
            var itemButton = go.GetComponent<ItemButton>();
            
            itemButton.Init(dir.Value);
            itemButton.OnAddButtonClick += controller.SelectExpItem;
            itemButton.OnSubtractButtonClick += controller.DeselectExpItem;
        }
    }

    public void ClearSpiritStoneScrollView()
    {
        for (int i = spiritStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = spiritStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
