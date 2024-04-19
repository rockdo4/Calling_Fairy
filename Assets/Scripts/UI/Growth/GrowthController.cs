using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthController : MonoBehaviour
{
    public FairyCard SelectFairy {  get; set; }

    [SerializeField]
    private List<GrowthView> infoViews = new List<GrowthView>();
    [SerializeField]
    private List<GrowthView> views = new List<GrowthView>();    

    [SerializeField]
    private TabGroup tabGroup;

    [Header("Level Up")]
    public Button lvUpButton;
    public ParticleSystem lvUpParticle;
    public LinkedList<ItemButton> SelectedExpItems { get; private set; } = new LinkedList<ItemButton>();

    [Header("Break Limit")]
    public ParticleSystem breakLimitParticle;

    public EquipSlot SelectedSlot { get; set; } = null;

    [Header("Equip Growth")]
    public LvUpEquipView lvUpEquipview;
    public Button equipLvUpButton;
    public GameObject rankUpAttractors;
    public List<ParticleSystem> rankUpParticles;
    public ParticleSystem equipExpParticle;
    public ParticleSystem fairyAttractorParticle2;
    private int equipParticleCount = 0;
    public LinkedList<ItemButton> SelectedEquipExpItems { get; private set; } = new LinkedList<ItemButton>();


    private void OnEnable()
    {
        tabGroup?.OnTabSelected(0);
    }

    public void SelectExpItem(ItemButton item)
    {
        if (!SelectedExpItems.Contains(item))
            SelectedExpItems.AddLast(item);

        Tuple<bool, Tuple<int, int>> result = CalculateExpOutcome(CalculatorTotalExp());

        if (result.Item1)
        {
            var view = views[1].GetComponent<LvUpView>();
            view.UpdateStatText(result.Item2.Item1, result.Item2.Item2);
            lvUpButton.interactable = true;
        }
        else
        {
            item.CountDown();
        }
    }

    public void DeselectExpItem(ItemButton item)
    {
        if (!SelectedExpItems.Contains(item))
            return;

        if (item.Count == 0)
            SelectedExpItems.Remove(item);

        var result = CalculateExpOutcome(CalculatorTotalExp());

        if (result.Item1)
        {
            var view = views[1].GetComponent<LvUpView>();
            view.UpdateStatText(result.Item2.Item1, result.Item2.Item2);
        }

        if (SelectedExpItems.Count == 0)
            lvUpButton.interactable= false;
    }

    // 반환: 성공 여부, 레벨, 경험치
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

        return Tuple.Create(sampleExp < table.dic[sampleLevel].Exp, Tuple.Create(sampleLevel, sampleExp));
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
            isBouns |= PropertyEquals(SelectFairy.ID, button.itemIcon.inventoryItem.ID);
        };

        lvUpButton.interactable = false;
        SaveLoadSystem.AutoSave();

        OpenLevleUpPopup(beforeLevel, isBouns);

        views[1].UpdateUI();
        infoViews[0].UpdateUI();
    }

    public int CalculatorTotalExp()
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        int totalExp = 0;

        foreach (var item in SelectedExpItems)
        {
            if (PropertyEquals(SelectFairy.ID, item.itemIcon.inventoryItem.ID))
            {
                totalExp += (int)((itemTable.dic[item.itemIcon.inventoryItem.ID].value2 * 1.5f) * item.Count);
            }
            else
            {
                totalExp += itemTable.dic[item.itemIcon.inventoryItem.ID].value2 * item.Count;
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
        return grade * 10 + 10 > level;
    }

    #region Break Limit
    public void TryShowBreakLimitEffect()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();

        if (InvManager.itemInv.Inven[10003].Count >= table.dic[SelectFairy.Grade].CharPieceNeeded)
        {
            UIManager.Instance.blockPanel.SetActive(true);
            breakLimitParticle.Play();
        }
    }

    public void TryBreakLimit()
    {
        if (breakLimitParticle.particleCount <= 1)
        {
            var table = DataTableMgr.GetTable<BreakLimitTable>();
            var stringTable = DataTableMgr.GetTable<StringTable>();

            InvManager.RemoveItem(InvManager.itemInv.Inven[10003], table.dic[SelectFairy.Grade].CharPieceNeeded);

            SelectFairy.GradeUp();

            UIManager.Instance.breakLimitModal.OpenPopup(stringTable.dic[303].Value, (SelectFairy.Grade - 1).ToString(), (SelectFairy.Grade).ToString());
            UIManager.Instance.blockPanel.SetActive(false);

            views[2].UpdateUI();
            infoViews[0].UpdateUI();
        }
    }
    #endregion

    #region Create Equipment

    public void SetEquipView()
    {
        views[3].UpdateUI();
    }

    public void OpenItemDropStageInfoPopup()
    {
        if (SelectedSlot == null)
            return;
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[SelectFairy.ID];
        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{SelectFairy.Rank}");
        var equipTable = DataTableMgr.GetTable<EquipTable>();

        UIManager.Instance.stageInfoModal.OpenPopup("드랍 스테이지 정보", equipTable.dic[key].EquipPiece);
    }

    public void EquipItem()
    {
        if (SelectedSlot == null)
            return;

        var charData = DataTableMgr.GetTable<CharacterTable>().dic[SelectFairy.ID];
        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{SelectFairy.Rank}");
        var newEquipment = new Equipment(key);

        SelectedSlot.CreateAndSetEquipment(newEquipment);
        SelectFairy.SetEquip(SelectedSlot.slotNumber, newEquipment);
        SetEquipView();
    }

    #endregion


    #region Equip Growth

    public void SelectEquipExpItem(ItemButton item)
    {
        if (!SelectedEquipExpItems.Contains(item))
            SelectedEquipExpItems.AddLast(item);

        var equipTable = DataTableMgr.GetTable<EquipTable>();
        var result = CalculateEquipExpOutcome(CalculatorEquipTotalExp());

        if (result.Item1)
        {
            lvUpEquipview.SetEquipGrowthInfoBox(equipTable.dic[SelectedSlot.Equipment.ID], result.Item2.Item1, result.Item2.Item2);
            equipLvUpButton.interactable = true;
        }
        else
        {
            item.CountDown();
        }
    }

    public void DeselectEquipExpItem(ItemButton item)
    {
        if (!SelectedEquipExpItems.Contains(item))
            return;

        if (item.Count == 0)
            SelectedEquipExpItems.Remove(item);

        var equipTable = DataTableMgr.GetTable<EquipTable>();
        var result = CalculateEquipExpOutcome(CalculatorEquipTotalExp());

        if (result.Item1)
        {
            lvUpEquipview.SetEquipGrowthInfoBox(equipTable.dic[SelectedSlot.Equipment.ID], result.Item2.Item1, result.Item2.Item2);
        }

        if (SelectedEquipExpItems.Count == 0)
            equipLvUpButton.interactable = false;
    }

    public int CalculatorEquipTotalExp()
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        int totalExp = 0;

        foreach (var item in SelectedEquipExpItems)
        {
            totalExp += itemTable.dic[item.itemIcon.inventoryItem.ID].value2 * item.Count;
        }

        return totalExp;
    }

    // 반환: 성공 여부, 레벨, 경험치
    public Tuple<bool, Tuple<int, int>> CalculateEquipExpOutcome(int exp)
    {
        var sampleLevel = SelectedSlot.Equipment.Level;
        var sampleExp = SelectedSlot.Equipment.Exp;

        sampleExp += exp;

        var table = DataTableMgr.GetTable<EquipExpTable>();

        while (sampleExp >= table.dic[sampleLevel].Exp && SelectedSlot.Equipment.Level < 30)
        {
            sampleExp -= table.dic[sampleLevel].Exp;
            sampleLevel++;
        }

        return Tuple.Create(sampleExp < table.dic[sampleLevel].Exp, Tuple.Create(sampleLevel, sampleExp));
    }

    public Stat EquipStatCalculator(EquipData data, int lv)
    {
        Stat result = new Stat();

        result.attack = (data.EquipAttack + data.EquipAttackIncrease * lv - 1) < 0 ? 0 : data.EquipAttack + data.EquipAttackIncrease * lv - 1;
        result.pDefence = (data.EquipPDefence + data.EquipPDefenceIncrease * lv - 1) < 0 ? 0 : data.EquipPDefence + data.EquipPDefenceIncrease * lv - 1;
        result.mDefence = (data.EquipMDefence + data.EquipMDefenceIncrease * lv - 1) < 0 ? 0 : data.EquipMDefence + data.EquipMDefenceIncrease * lv - 1;
        result.hp = (data.EquipMaxHP + data.EquipHPIncrease * lv - 1) < 0 ? 0 : data.EquipMaxHP + data.EquipHPIncrease * lv - 1;

        return result;
    }

    public void ResetSelectedSlot()
    {
        SelectedSlot = null;
    }

    public void TryShowEquipLvUpEffect()
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return;

        UIManager.Instance.blockPanel.SetActive(true);
        equipExpParticle.Play();
    }

    public void TryEquipLvUp()
    {
        if (SelectedEquipExpItems.Count == 0)
            return;

        if (equipExpParticle.particleCount > 1)
            return;

        int beforeLevel = SelectedSlot.Equipment.Level;

        SelectedSlot.Equipment.AddExperience(CalculatorEquipTotalExp());

        foreach (var button in SelectedEquipExpItems)
        {
            button.UseItem();
        };

        equipLvUpButton.interactable = false;
        UIManager.Instance.blockPanel.SetActive(false);

        SaveLoadSystem.AutoSave();

        var strigTable = DataTableMgr.GetTable<StringTable>();
        var statsName = $"{strigTable.dic[305].Value}\n{strigTable.dic[306].Value}\n{strigTable.dic[307].Value}\n{strigTable.dic[308].Value}\n{strigTable.dic[313].Value}";
        UIManager.Instance.lvUpModal.OpenPopup($"{strigTable.dic[302].Value}", $"{SelectedSlot.Equipment.Level} ", SelectedSlot.Equipment.Exp,
            DataTableMgr.GetTable<EquipExpTable>().dic[SelectedSlot.Equipment.Level].Exp, statsName, GetEquipLvUpResult(beforeLevel, SelectedSlot.Equipment.Level), strigTable.dic[1].Value, null, false);

        views[3].UpdateUI();
        infoViews[1].UpdateUI();
    }

    public string GetEquipLvUpResult(int beforeLv, int afterLv)
    {
        var equipData = DataTableMgr.GetTable<EquipTable>().dic[SelectedSlot.Equipment.ID];

        var beforeStat = EquipStatCalculator(equipData, beforeLv);
        var afterStat = EquipStatCalculator(equipData, afterLv);

        return $"{beforeLv} -> {afterLv}\n" +
            $"{beforeStat.attack} -> {afterStat.attack}\n" +
            $"{beforeStat.hp} -> {afterStat.hp}\n" +
            $"{beforeStat.pDefence} -> {afterStat.pDefence}\n" +
            $"{beforeStat.mDefence} -> {afterStat.mDefence}";
    }

    public void TryShowRankUpEffect()
    {
        if (SelectFairy.equipSocket.Count != 6)
            return;

        foreach (var value in SelectFairy.equipSocket.Values)
        {
            if (value == null)
                return;
        }

        rankUpAttractors.SetActive(true);

        foreach (var particle in rankUpParticles)
        {
            UIManager.Instance.blockPanel.SetActive(true);
            particle.Play();
        }
    }

    public void TryRankUp()
    {
        equipParticleCount++;

        if (equipParticleCount < 6)
            return;

        StartCoroutine(WaitForParticleCompletionThenRankUp(fairyAttractorParticle2));
    }

    IEnumerator WaitForParticleCompletionThenRankUp(ParticleSystem particle)
    {

        yield return new WaitForSeconds(particle.main.duration);

        equipParticleCount = 0;
        SelectFairy.RankUp();
        ResetSelectedSlot();

        views[3].UpdateUI();
        infoViews[1].UpdateUI();

        rankUpAttractors.SetActive(false);
        UIManager.Instance.blockPanel.SetActive(false);
    }

    #endregion
}
