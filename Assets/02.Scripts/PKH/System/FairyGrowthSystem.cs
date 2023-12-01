using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthSystem : MonoBehaviour
{
    public struct Stat
    {
        public int attack;
        public int pDefence;
        public int mDefence;
        public int hp;
    }

    public FairyCard Card { get; set; }
    public UI ui;

    [Header("LvUp")]
    public TextMeshProUGUI lvGrowthText;
    public TextMeshProUGUI infoPanel;
    public Transform spiritStoneSpace;

    [Header("BreakLimit")]
    public TextMeshProUGUI originGrade;
    public TextMeshProUGUI nextGrade;
    public TextMeshProUGUI pieceProgress;
    public ItemIcon pieceIcon;

    [Header("Common")]
    public GameObject itemButtonPrefab;
    public GameObject itemIconPrefab;

    private int sampleLv;
    private int sampleExp;
    private CharData charData;
    private ExpTable expTable; 
    private List<ItemButton> itemButtons = new List<ItemButton>();

    public void Awake()
    {
        ui.OnActive += SetSample;
        ui.OnActive += ClearLvUpView;
        ui.OnNonActive += ClearLvUpView;

        expTable = DataTableMgr.GetTable<ExpTable>();
    }

    public void Init(FairyCard card)
    {
        Card = card;
        charData = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];
        SetSample();
        SetLeftPanel();
        SetRightPanel();
    }
   

    public void SetLeftPanel()
    {
        if (Card is FairyCard)
        {
            UpdataInfoPanel();
        }
        else
        {
            //Set SupCard
        }
    }

    public void SetRightPanel()
    {
        if (Card is FairyCard)
        {
            SetLvUpView();
            //SetBreakLimitView();
        }
        else
        {
            //Set SupCard
        }
    }

    public void UpdataInfoPanel()
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
        ClearLvUpView();
        UpdateStatText(Card.Level, Card.Experience);

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

    public void ClearLvUpView()
    {
        for (int i = spiritStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = spiritStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void Simulation(Item item)
    {
        var spiritStone = item as SpiritStone;
        var table = DataTableMgr.GetTable<ExpTable>();

        sampleExp += spiritStone.Exp;

        if (sampleExp >= table.dic[sampleLv].Exp && CheckGrade(Card.Grade, sampleLv))
        {
            sampleExp -= table.dic[sampleLv].Exp;
            sampleLv++;
        }
        UpdateStatText(sampleLv, sampleExp);
    }

    public void LvUp()
    {
        if (sampleExp <= Card.Level || !CheckGrade(Card.Grade, Card.Level))
            return;

        Card.LevelUp(sampleLv, sampleExp);

        foreach (var button in itemButtons)
        {
            button.UseItem();
        }
        SetLvUpView();
        UpdataInfoPanel();
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
                //»•«’(πÃ¡§)
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
            //SetBreakLimitView();
        } 
    }

    #endregion

}