using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthSystem : MonoBehaviour
{
    public FairyCard Card { get; set; }
    public UI ui;

    //LvUpSimulation
    public TextMeshProUGUI lvGrowthText;
    public Transform spiritStoneSpace;
    public GameObject iconPrefab;

    private int sampleID;
    private int sampleEx;
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
        sampleEx = Card.Experience;
        sampleID = Card.ID;
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
            SetLvUpView(Card.ID);

            //for (int i = )
        }
        else
        {
            //Set SupCard
        }
    }

    public void UpdateStatText(int id)
    {
        var dic = DataTableMgr.GetTable<CharacterTable>().dic[id];
        //lvGrowthText.text = $"Lv: {dic.CharLevel,-10}\t\tEx: {Card.Experience,-10}\n" +
        //    $"Attack: {dic.CharPAttack,-10}\t\tMaxHP: {dic.CharMaxHP,-10}";
    }

    public void SetLvUpView(int id)
    {
        ClearLvUpView();
        UpdateStatText(id);

        var dic = DataTableMgr.GetTable<CharacterTable>().dic[id];
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

    public void Simulation(ItemButton ib)
    {
        var spiritStone = ib.itemIcon.item as SpiritStone;
        sampleEx += spiritStone.Ex;

        var table = DataTableMgr.GetTable<CharacterTable>();
        if (sampleEx < table.dic[sampleID].CharExp)
            return;

        if (Card.grade < table.dic[table.dic[sampleID].CharNextLevel.ToString()].CharMinGrade)
            return;

        sampleEx -= table.dic[sampleID].CharExp;
        sampleID = table.dic[sampleID].CharNextLevel;

        UpdateStatText(sampleID);
    }

    public void LvUp()
    {
        Card.ID = sampleID;
        Card.Experience = sampleEx;
        foreach (var button in spiritButtons)
        {
            button.UseItem();
        }
        SetLvUpView(Card.ID);
    }

}
