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
        public int Attack;
        public int Defence;
        public int Hp;
    }

    public FairyCard Card { get; set; }
    public UI ui;

    //LvUpSimulation
    public TextMeshProUGUI lvGrowthText;
    public Transform spiritStoneSpace;
    public GameObject iconPrefab;

    private int sampleLv;
    private int sampleExp;
    private Stat originStat;
    private Stat equipStat;
    private List<ItemButton> spiritButtons = new List<ItemButton>();

    public void Awake()
    {
        ui.OnActive += SetSample;
        ui.OnActive += ClearLvUpView;
        ui.OnNonActive += ClearLvUpView;
    }

    public void Init(FairyCard card)
    {
        Card = card;
        SetSample();
        SetLeftPanel();
        SetRightPanel();
    }


    //LvUpButton에 추가하기
    public void SetSample()
    {
        sampleLv = Card.Level;
        sampleExp = Card.Experience;
    }


    public void SetLeftPanel()
    {
        if (Card is FairyCard)
        {
            
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

            //for (int i = )
        }
        else
        {
            //Set SupCard
        }
    }

    public void UpdateStatText(int level, int exp)
    {
        var dic = DataTableMgr.GetTable<CharacterTable>().dic[Card.ID];

        if (dic.CharAttackType == 1)
        {
            lvGrowthText.text = $"Lv: {level,-10}\t\tEx: {exp,-10}\n" +
            $"Attack: {dic.CharPAttack,-10}\t\tMaxHP: {dic.CharMaxHP,-10}";
        }
        else if (dic.CharAttackType == 2)
        {
            lvGrowthText.text = $"Lv: {level,-10}\t\tEx: {exp,-10}\n" +
            $"Attack: {dic.CharMAttack,-10}\t\tMaxHP: {dic.CharMaxHP,-10}";
        }
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
                
            var go = Instantiate(iconPrefab, spiritStoneSpace);
            var ib = go.GetComponent<ItemButton>();
            spiritButtons.Add(ib);
            var button = go.GetComponent<Button>();
            ib.itemIcon.item = dir.Value;
            ib.SetButton();
            ib.OnClick += Simulation;
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
        sampleExp += spiritStone.Exp;

        var table = DataTableMgr.GetTable<ExpTable>();
        if (sampleExp < table.dic[sampleLv].Exp)
            return;

        if (!CheckGrade(Card.Grade, Card.Level))
            return;

        sampleExp -= table.dic[sampleLv].Exp;
        sampleLv++;

        UpdateStatText(sampleLv, sampleExp);
    }

    public void LvUp()
    {
        Card.Level = sampleLv;
        Card.Experience = sampleExp;
        foreach (var button in spiritButtons)
        {
            button.UseItem();
        }
        SetLvUpView();
    }

    public bool CheckGrade(int grade, int level)
    {
        return grade * 10 + 10 > level;
    }

}
