using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.Progress;

public class GrowthController : MonoBehaviour
{
    public FairyCard SelectFairy {  get; set; }

    private List<GrowthView> views = new List<GrowthView>();    

    [SerializeField]
    private TabGroup tabGroup;

    [Header("Level Up")]
    public Button lvUpButton;
    public ParticleSystem lvUpParticle;
    public LinkedList<ItemButton> SelectedExpItems { get; private set; } = new LinkedList<ItemButton>();

    public void SelectExpItem(ItemButton item)
    {
        if (SelectedExpItems.Contains(item))
            return;

        SelectedExpItems.AddLast(item);
    }

    public void DeselectExpItem(ItemButton item)
    {
        if (!SelectedExpItems.Contains(item))
            return;

        if (item.Count == 0)
            SelectedExpItems.Remove(item);
    }

    public bool ExpSimulation()
    {
        var result = CalculateExpOutcome(CalculatorTotalExp());

        return result.Item1;
    }

    public Tuple<bool, Tuple<int, int>> CalculateExpOutcome(int exp)
    {
        var sampleLevel = SelectFairy.Level;
        var sampleExp = SelectFairy.Experience;

        sampleExp += exp;

        var table = DataTableMgr.GetTable<ExpTable>();

        while (sampleExp >= table.dic[sampleLevel].Exp && CheckGrade(SelectFairy.Grade, sampleLevel))
        {
            sampleExp -= table.dic[sampleLevel].Exp;
            sampleLevel++;
        }

        return Tuple.Create(sampleExp < table.dic[sampleLevel].Exp, Tuple.Create(sampleExp, sampleLevel));
    }

    public Stat StatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.CharAttack + data.CharAttackIncrease * lv;
        result.pDefence = data.CharPDefence + data.CharPDefenceIncrease * lv;
        result.mDefence = data.CharMDefence + data.CharMDefenceIncrease * lv;
        result.hp = data.CharMaxHP + data.CharHPIncrease * lv;

        return result;
    }

    public void TryShowLevelUpEffect()
    {
        if (!CheckGrade(SelectFairy.Grade, SelectFairy.Level))
            return;

        UIManager.Instance.blockPanel.SetActive(true);
        lvUpParticle.Play();
    }

    public void TryAddExp()
    {
        if (SelectedExpItems.Count == 0)
            return;

        if (lvUpParticle.particleCount > 1)
            return;

        int beforeLevel = SelectFairy.Level;

        SelectFairy.AddExperience(CalculatorTotalExp());

        bool isBouns = false;

        foreach (var button in SelectedExpItems)
        {
            button.UseItem();
            isBouns |= PropertyEquals(SelectFairy.ID, button.inventoryItem.ID);
        };

        lvUpButton.interactable = false;
        SaveLoadSystem.AutoSave();


        OpenLevleUpPopup(beforeLevel, isBouns);
        //SetLvUpView();
        //leftCardView.Init(Card);
    }

    public int CalculatorTotalExp()
    {
        var charTable = DataTableMgr.GetTable<CharacterTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        int totalExp = 0;

        foreach (var item in SelectedExpItems)
        {
            if (PropertyEquals(SelectFairy.ID, item.inventoryItem.ID))
            {
                totalExp += (int)((itemTable.dic[item.inventoryItem.ID].value2 * 1.5f) * item.Count);
            }
            else
            {
                totalExp += itemTable.dic[item.inventoryItem.ID].value2 * item.Count;
            }
        }

        return totalExp;
    }

    public bool PropertyEquals(int fairyId, int itemId)
    {
        var charTable = DataTableMgr.GetTable<CharacterTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        return charTable.dic[fairyId].CharProperty == itemTable.dic[itemId].value1;
    }

    public void OpenLevleUpPopup(int beforeLevel, bool isBonus)
    {
        UIManager.Instance.blockPanel.SetActive(false);

        var stringTable = DataTableMgr.GetTable<StringTable>();
        var statsName = $"{stringTable.dic[305].Value}\n{stringTable.dic[306].Value}\n{stringTable.dic[307].Value}\n{stringTable.dic[308].Value}\n{stringTable.dic[313].Value}";

        UIManager.Instance.lvUpModal.OpenPopup(stringTable.dic[302].Value, SelectFairy.Level.ToString(), SelectFairy.Experience,
            DataTableMgr.GetTable<ExpTable>().dic[SelectFairy.Level].Exp, statsName, GetLvUpResult(beforeLevel, SelectFairy.Level), stringTable.dic[1].Value, null, isBonus);
    }

    public string GetLvUpResult(int beforeLv, int afterLv)
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[SelectFairy.ID];

        var beforeStat = StatCalculator(charData, beforeLv);
        var afterStat = StatCalculator(charData, afterLv);

        return $"{beforeLv} -> {afterLv}\n" +
            $"{beforeStat.attack} -> {afterStat.attack}\n" +
            $"{beforeStat.hp} -> {afterStat.hp}\n" +
            $"{beforeStat.pDefence} -> {afterStat.pDefence}\n" +
            $"{beforeStat.mDefence} -> {afterStat.pDefence}";
    }

    public bool CheckGrade(int grade, int level)
    {
        return grade * 10 + 10 >= level;
    }
}
