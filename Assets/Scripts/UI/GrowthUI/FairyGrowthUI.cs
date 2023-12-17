using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthUI : UI
{
    [Header("Common")]
    public GameObject itemButtonPrefab;
    public GameObject itemIconPrefab;
    public View leftCardView;
    public View leftEquipView;
    public TextMeshProUGUI infoPanel;
    public TabGroup tabGroup;
    public List<Tab> tabButtons;

    public FairyCard Card { get; set; }

    private CharData charData;
    private ExpTable expTable;

    [Header("Stat Info")]
    public View statInfoView;

    [Header("LvUp")]
    public View lvUpView;
    //객체화 예정
    public TextMeshProUGUI lvText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pDefenceText;
    public TextMeshProUGUI mDefenceText;
    public TextMeshProUGUI expText;
    public Image expSlider;
    //
    public Transform spiritStoneSpace;
    public bool LvUpLock { get; set; }

    private int sampleLv;
    private int sampleExp;
    private List<ItemButton> itemButtons = new List<ItemButton>();


    [Header("BreakLimit")]
    public TextMeshProUGUI originGrade;
    public TextMeshProUGUI nextGrade;
    public TextMeshProUGUI pieceProgress;
    public ItemIcon pieceIcon;


    [Header("Equip")]
    public Transform enforceStoneSpace;
    public EquipInfoBox equipInfoBox;

    public EquipSlot SelectedSlot { get; set; } = null;

    private int equipSampleLv;
    private int equipSampleExp;
    private List<ItemButton> enforceStoneButtons = new List<ItemButton>();


    public void Awake()
    {
        expTable = DataTableMgr.GetTable<ExpTable>();
    }

    public override void ActiveUI()
    {
        base.ActiveUI();
    }

    //선택한 카드로 UI 초기화
    public void Init(FairyCard card)
    {
        Card = card;
        charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
        tabGroup?.OnTabSelected(tabButtons?[0]);
        SelectedSlot = null;
        SetLeftPanel();
        SetRightPanel();
    }
   
    public void SetLeftPanel()
    {
        if (tabGroup.selectedTab.Equals(tabButtons?[3]))
        {
            leftCardView.gameObject.SetActive(false);
            leftEquipView.gameObject.SetActive(true);
            leftEquipView.Init(Card);
        }
        else
        {
            leftEquipView.gameObject.SetActive(false);
            leftCardView.gameObject.SetActive(true);
            leftCardView.Init(Card);
        } 
    }

    public void SetRightPanel()
    {
        if (tabGroup.selectedTab == tabButtons[0])
        {
            statInfoView.Init(Card);
        }
        else
        if (tabGroup.selectedTab == tabButtons[1])
        {
            //lvUpVies.Init(Card);
            SetLvUpView();
        }
        else if (tabGroup.selectedTab == tabButtons[2])
        {
            SetBreakLimitView();
        }
        else if (tabGroup.selectedTab == tabButtons[3])
        {
            SetEquipView();
        }
    }

    //객체화 하기
    public void SetCardInfoView()
    {
        infoPanel.text = $"Name: {charData.CharName,-20}Grade: {Card.Grade,-10}{charData.CharProperty}/{charData.CharPosition}\n" +
            $"Lv {Card.Level,-20}{Card.Experience}/{expTable.dic[Card.Level].Exp}";
    }

    #region LvUP
    public void SetSample()
    {
        sampleLv = Card.Level;
        sampleExp = Card.Experience;
    }
    public void UpdateStatText(int level, int exp)
    {
        var stat = StatCalculator(charData, level);
        lvText.text = level.ToString();
        attackText.text = stat.attack.ToString();
        hpText.text = stat.hp.ToString();
        pDefenceText.text = stat.pDefence.ToString();
        mDefenceText.text = stat.mDefence.ToString();
        expText.text = $"{exp} / {expTable.dic[level].Exp}";
        expSlider.fillAmount = (float)exp / expTable.dic[level].Exp;
    }
    public void SetLvUpView()
    {
        ClearSpiritStoneScrollView();
        SetSample();
        UpdateStatText(Card.Level, Card.Experience);
        SetSpiritStoneScroolView();
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
            itemButtons.Add(itemButton);
            itemButton.Init(dir.Value);
            itemButton.OnClick += Simulation;
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

    public bool Simulation(Item item)
    {
        var table = DataTableMgr.GetTable<ExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        
        if (!CheckGrade(Card.Grade, sampleLv))
        {
            return false;
        }

        if (itemTable.dic.TryGetValue(item.ID, out var itemData))
        {
            sampleExp += itemData.value2;
        }
        
        if (sampleExp >= table.dic[sampleLv].Exp)
        {
            sampleExp -= table.dic[sampleLv].Exp;
            sampleLv++;
        }
        UpdateStatText(sampleLv, sampleExp);
        return true;
    }

    public void LvUp()
    {
        if (!CheckGrade(Card.Grade, Card.Level))
            return;

        if (sampleLv == Card.Level)
            return;

        Card.LevelUp(sampleLv, sampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        }
        SetLvUpView();
        leftCardView.Init(Card);
    }

    public bool CheckGrade(int grade, int level)
    {
        return grade * 10 + 10 > level;
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
    #endregion

    #region Break Limit
    public void UpdataGradeText(int grade, TextMeshProUGUI text)
    {
        text.text = $"Grade: {grade}\nMaxLv: {grade * 10 + 10}";
    }

    public void SetPieceIcon()
    {
        if (InvManager.itemInv.Inven.TryGetValue(10003, out Item item))  //memoryPiece ID
        {
            var itemIcon = pieceIcon.GetComponent<ItemIcon>();
            itemIcon.Init(item);
        }
    }

    public void SetGradeProgress()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();
        if (InvManager.itemInv.Inven.TryGetValue(10003, out Item item))
        {
            pieceProgress.text = $"{item.Count} / {table.dic[Card.Grade].CharPieceNeeded}";
        }
        else
        {
            pieceProgress.text = $"0 / {table.dic[Card.Grade].CharPieceNeeded}";
        }
    }

    public void SetBreakLimitView()
    {
        UpdataGradeText(Card.Grade, originGrade);
        int ng = Card.Grade > 5 ? Card.Grade : Card.Grade + 1;
        UpdataGradeText(ng, nextGrade);
        SetPieceIcon();
        SetGradeProgress();
    }

    public void BreakLimit()
    {
        var table = DataTableMgr.GetTable<BreakLimitTable>();
        if (InvManager.itemInv.Inven[10003].Count >= table.dic[Card.Grade].CharPieceNeeded)
        {
            InvManager.itemInv.Inven[10003].Count -= table.dic[Card.Grade].CharPieceNeeded;
            Card.Grade = Card.Grade < 5 ? Card.Grade + 1 : Card.Grade;

            SetLeftPanel();
            SetBreakLimitView();
        } 
    }

    #endregion

    #region EquipTap

    public void SetEquip()
    {
        if (SelectedSlot == null)
            return;

        var position = charData.CharPosition;
        var key = Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");
        var newEquipment = new Equipment(key);

        SelectedSlot.CreateAndSetEquipment(newEquipment);
        Card.SetEquip(SelectedSlot.slotNumber, newEquipment);
    }


    public void RankUp()
    {
        if (Card.equipSocket.Count == 6)
            return;

        foreach (var value in Card.equipSocket.Values)
        {
            if (value == null) 
                return;
        }
        Card.RankUp();
        SetLeftPanel();
        SetEquipView();
    }

    public void SetEquipView()
    {
        ClearEnforceStoneScrollView();
        SetEnforceStoneScroolView();
        SetEquipSample(SelectedSlot?.Equipment);
        equipInfoBox.SetEquipInfo(SelectedSlot?.Equipment);
        leftEquipView.Init(Card);
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
                    var go = Instantiate(itemButtonPrefab, enforceStoneSpace);
                    var itemButton = go.GetComponent<ItemButton>();
                    enforceStoneButtons.Add(itemButton);
                    itemButton.Init(enforceStone);
                    itemButton.OnClick += EquipSimulation;
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
        enforceStoneButtons.Clear();
    }

    public bool EquipSimulation(Item item)
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return false;

        if (equipSampleLv >= 30)
            return false;

        var expTable = DataTableMgr.GetTable<EquipExpTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (itemTable.dic.TryGetValue(item.ID, out ItemData itemData))
        {
            equipSampleExp += itemData.value2;
        }

        if (equipSampleExp >= expTable.dic[equipSampleLv].Exp)
        {
            equipSampleExp -= expTable.dic[equipSampleLv].Exp;
            equipSampleLv++;
        }
        equipInfoBox.SetEquipInfo(SelectedSlot?.Equipment, equipSampleLv, equipSampleExp);
        return true;
    }

    public void SetEquipSample(Equipment equipment)
    {
        if (equipment == null) 
            return;

        equipSampleLv = equipment.Level;
        equipSampleExp = equipment.Exp;
    }

    public void EquipLvUp()
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return;

        if (equipSampleLv >= 30)
            return;

        SelectedSlot.Equipment.LevelUp(equipSampleLv, equipSampleExp);

        foreach (var button in enforceStoneButtons)
        {
            button.UseItem();
        }
        
        leftEquipView.Init(Card);
    }

    #endregion
}