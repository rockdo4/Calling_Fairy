using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyGrowthSystem : MonoBehaviour
{
    public FairyCard card;

    //LvUpSimulation
    public TextMeshProUGUI lvGrowthText;
    public Transform spiritStoneSpace;
    public GameObject iconPrefab;

    private int sampleID;
    private int sampleEx;
    private List<ItemButton> spiritButtons = new List<ItemButton>();

    public void ActiveUI(FairyCard card)
    {
        this.card = card;
        ClearLvUpView();
        SetSample();
        gameObject.SetActive(true);
    }

    public void NonActiveUI()
    {
        card = null;
        ClearLvUpView();
        gameObject.SetActive(false);
    }

    //LvUpButton에 추가하기
    public void SetSample()
    {
        sampleEx = card.Experience;
        sampleID = card.ID;
    }

    public void SetLeftPanel(Card card)
    {
        if (card is FairyCard)
        {
            
        }
        else
        {
            //Set SupCard
        }
    }

    public void SetRightPanel()
    {
        if (card is FairyCard)
        {
            SetLvUpView(card.ID);

            //for (int i = )
        }
        else
        {
            //Set SupCard
        }
    }

    public void UpdateStatText(int id)
    {
        var dic = DataTableMgr.GetTable<CharacterTable>().dic[id.ToString()];
        lvGrowthText.text = $"Lv: {dic.CharLevel,-10}\t\tEx: {card.Experience,-10}\n" +
            $"Attack: {dic.CharPAttack,-10}\t\tMaxHP: {dic.CharMaxHP,-10}";
    }

    public void SetLvUpView(int id)
    {
        ClearLvUpView();
        UpdateStatText(id);

        var dic = DataTableMgr.GetTable<CharacterTable>().dic[id.ToString()];
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
        if (sampleEx < table.dic[sampleID.ToString()].CharExp)
            return;

        if (card.grade < table.dic[table.dic[sampleID.ToString()].CharNextLevel.ToString()].CharMinGrade)
            return;

        sampleEx -= table.dic[sampleID.ToString()].CharExp;
        sampleID = table.dic[sampleID.ToString()].CharNextLevel;

        UpdateStatText(sampleID);
    }

    public void LvUp()
    {
        card.ID = sampleID;
        card.Experience = sampleEx;
        foreach (var button in spiritButtons)
        {
            button.UseItem();
        }
        SetLvUpView(card.ID);
    }

}
