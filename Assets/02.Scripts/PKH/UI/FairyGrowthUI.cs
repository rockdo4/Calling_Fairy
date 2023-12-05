using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FairyGrowthUI : UI
{
    public struct Stat
    {
        public int attack;
        public int pDefence;
        public int mDefence;
        public int hp;
    }

    [Header("Common")]
    public GameObject itemButtonPrefab;
    public GameObject itemIconPrefab;
    public View leftCardView;
    public View leftEquipView;
    public TextMeshProUGUI infoPanel;

    public FairyCard Card { get; set; }

    private CharData charData;
    private ExpTable expTable;
    private GameObject currentLeftView;
    private TabGroup tabGroup;

    [Header("LvUp")]
    public TextMeshProUGUI lvGrowthText;
    public Transform spiritStoneSpace;
    public bool Limit { get; set; }

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

    public EquipSlot SelectedSlot { get; set; } = null;

    private int equipSampleLv;
    private int equipSampleExp;
    private List<ItemButton> enforceStoneButtons = new List<ItemButton>();


    public void Awake()
    {
        tabGroup = GetComponentInChildren<TabGroup>();
        expTable = DataTableMgr.GetTable<ExpTable>();
    }

    //������ ī��� UI �ʱ�ȭ
    public void Init(FairyCard card)
    {
        Card = card;
        charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
        SetLeftPanel();
        SetRightPanel();
    }
   
    public void SetLeftPanel()
    {
        SetCardInfoView();
        leftCardView.Init(Card);
        leftEquipView.Init(Card);
    }

    public void SetRightPanel()
    {
        SetLvUpView();
        SetBreakLimitView();
    }
    //��üȭ �ϱ�
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
        lvGrowthText.text = $"Lv: {level,-10}\t\tEx: {exp,-10} / {expTable.dic[level].Exp}\n" +
            $"Attack: {stat.attack,-10}\t\tMaxHP: {stat.hp,-10}\n" +
            $"PDefence: {stat.pDefence,-10}\t\tMDefence: {stat.mDefence,-10}";
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

        //���ɼ� id�� ����ġ �������� + �Ӽ� üũ�� �ϱ�

        Limit = !CheckGrade(Card.Grade, sampleLv);
        if (Limit)
        {
            return Limit;
        }

        //sampleExp += spiritStone.Exp;
        if (sampleExp >= table.dic[sampleLv].Exp)
        {
            sampleExp -= table.dic[sampleLv].Exp;
            sampleLv++;
        }
        UpdateStatText(sampleLv, sampleExp);
        return Limit;
    }

    public void LvUp()
    {
        if (!CheckGrade(Card.Grade, Card.Level))
            return;

        Card.LevelUp(sampleLv, sampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        }
        SetLvUpView();
        SetCardInfoView();
    }

    public bool CheckGrade(int grade, int level)
    {
        return grade * 10 + 10 > level;
    }

    public Stat StatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        switch (data.CharAttackType)
        {
            case 1:
                result.attack = data.CharPAttack + data.CharPAttackIncrease * lv;
                break;
            case 2:
                result.attack = data.CharMAttack + data.CharMAttackIncrease * lv;
                break;
            case 3:
                //ȥ��(����)
                break;
        }

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
        var key = System.Convert.ToInt32($"30{position}{SelectedSlot.slotNumber}0{Card.Rank}");

        SelectedSlot.CreateAndSetEquipment(new Equipment(key));
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
        SetRightPanel();
    }

    public void SetEquipView(Equipment equip)
    {
        SetEquipSample(equip);

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
    }

    public bool EquipSimulation(Item item)
    {
        if (SelectedSlot == null || SelectedSlot.Equipment == null)
            return false;

        var expTable = DataTableMgr.GetTable<EquipExpTable>();
        //var itemTable = DataTableMgr.GetTable<ItemTable>();

        if (equipSampleLv >= 30)
            return false;

        /*if (itemTable.dic.TryGetValue(item.ID, out ItemData itemData))
        {
            equipSampleExp += itemData.value2;
        }*/

        if (equipSampleExp >= expTable.dic[sampleLv].Exp)
        {
            equipSampleExp -= expTable.dic[sampleLv].Exp;
            equipSampleLv++;
        }
        
        return true;
    }

    public void SetEquipSample(Equipment equipment)
    {
        if (equipment == null) 
            return;

        equipSampleLv = equipment.Level;
        equipSampleExp = equipment.Exp;
    }

    public void EquipLvUp(Equipment equipment)
    {
        if (equipSampleLv >= 30)
            return;

        equipment.LevelUp(equipSampleLv, equipSampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        }
        SetEquipView(equipment);
        leftEquipView.Init(Card);
    }

    #endregion

}