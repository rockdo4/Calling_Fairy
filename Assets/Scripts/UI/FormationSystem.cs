using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FormationSystem : MonoBehaviour
{
    public SlotGroup fairyCardSlots;
    public SlotGroup supCardSlots;

    public SlotGroup SelectedGroup { get; set; }
    public void OnEnable()
    {
        CardStateInit();
    }

    public void CardStateInit()
    {
        foreach (var card in InvManager.fairyInv.Inven)
        {
            card.Value.IsUse = false;
        }
    }
    public void SendFairyCardsAndGameStart()
    {
        if (fairyCardSlots.slots[fairyCardSlots.slots.Count - 1].SelectedInvenItem == null)
            return;

        for (int i = 0; i < fairyCardSlots.slots.Count; i++)
        {
            GameManager.Instance.Team[i] = fairyCardSlots.slots[i].SelectedInvenItem as FairyCard;
        }
        //GameManager.Instance.SceneLoad("BattleScene");
    }

    //사거리 기준 정렬
    public void SortSetFairy(InventoryItem newItem)
    {
        if (SelectedGroup == null)
            return;

        var table = DataTableMgr.GetTable<CharacterTable>();
        var sortDic = new SortedDictionary<float, InventoryItem>();

        foreach (var slot in SelectedGroup.slots)
        {
            if (slot.SelectedInvenItem == null)
                break;
            sortDic.Add(table.dic[slot.SelectedInvenItem.ID].CharAttackRange, slot.SelectedInvenItem);
        }
        sortDic.Add(table.dic[newItem.ID].CharAttackRange, newItem);

        int index = 0;
        foreach (var dic in sortDic)
        {
            SelectedGroup.slots[index++].SetSlot(dic.Value);
        }
        SelectedGroup.SelectedSlot = null;
        SelectedGroup = null;

        var card = newItem as FairyCard;
        card.IsUse = true;
    }


    public void SortSetFairy2(InventoryItem newItem)
    {
        if (SelectedGroup == null)
            return;

        var table = DataTableMgr.GetTable<CharacterTable>();
        var sortDic = new SortedDictionary<float, List<InventoryItem>>();

        foreach (var slot in SelectedGroup.slots)
        {
            if (slot.SelectedInvenItem == null)
                break;

            float attackRange = table.dic[slot.SelectedInvenItem.ID].CharAttackRange;
            if (!sortDic.ContainsKey(attackRange))
            {
                sortDic[attackRange] = new List<InventoryItem>();
            }
            sortDic[attackRange].Add(slot.SelectedInvenItem);
        }

        float newItemRange = table.dic[newItem.ID].CharAttackRange;
        if (!sortDic.ContainsKey(newItemRange))
        {
            sortDic[newItemRange] = new List<InventoryItem>();
        }
        sortDic[newItemRange].Add(newItem);

        int index = 0;
        foreach (var pair in sortDic)
        {
            if (pair.Value.Count > 1)
            {
                var newArray = new int[pair.Value.Count];
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    
                    newArray[i] = table.dic[pair.Value[i].ID].CharPosition;
                }

                var array = pair.Value.ToArray();
                Array.Sort(newArray, array);

                pair.Value.Clear();
                pair.Value.AddRange(array);
            }

            foreach (var item in pair.Value)
            {
                SelectedGroup.slots[index++].SetSlot(item);
            }
        }
        SelectedGroup.SelectedSlot = null;
        SelectedGroup = null;

        var card = newItem as FairyCard;
        card.IsUse = true;
    }


    public void SortSlots()
    {
        if (SelectedGroup == null)
            return;

        Queue<InventoryItem> tempQueue = new Queue<InventoryItem>();
        foreach (var slot in SelectedGroup.slots)
        {
            if (slot.SelectedInvenItem == null)
                continue;
            tempQueue.Enqueue(slot.SelectedInvenItem);
            slot.UnsetSlot();
        }

        foreach (var slot in SelectedGroup.slots)
        {
            if(tempQueue.TryDequeue(out var item))
            {
                slot.SetSlot(item);
            }
            else
            {
                slot.UnsetSlot();
            }
        }
        SelectedGroup = null;
    }
}
