using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvUpSimulation : MonoBehaviour
{
    public TextMeshProUGUI growthText;
    public Transform spiritStoneSpace;
    public GameObject iconPrefab;

    private FairyCard card;

    public void SetView(Card card)
    {
        var dic = DataTableMgr.GetTable<CharacterTable>().dic[card.ID.ToString()];

        growthText.text = $"Lv: {dic.CharLevel,-10}\t\tEx: {card.Experience,-10}\n" +
            $"Attack: {dic.CharPAttack,-10}\t\tMaxHP: {dic.CharMaxHP,-10}";

        foreach (var dir in InvManager.spiritStoneInv.Inven)
        {
            var go = Instantiate(iconPrefab, spiritStoneSpace);
            var ib = go.GetComponent<ItemButton>();
            ib.itemIcon.item = dir.Value;
            ib.SetButton();
        }
    }

    public void Clear()
    {
        for (int i = spiritStoneSpace.childCount - 1; i >= 0; i--)
        {
            var child = spiritStoneSpace.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void Simulation()
    {
        Experience += ex;
        var table = DataTableMgr.GetTable<CharacterTable>();
        if (Experience < table.dic[ID.ToString()].CharExp)
            return;

        if (grade >= table.dic[table.dic[ID.ToString()].CharNextLevel.ToString()].CharMinGrade)
        {
            Experience = table.dic[ID.ToString()].CharExp;
            return;
        }

        Experience -= table.dic[ID.ToString()].CharExp;
        ID = table.dic[ID.ToString()].CharNextLevel;
    }
}
