using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FormationSystem : MonoBehaviour
{
    public SlotGroup fairyCardSlots;
    public SlotGroup supCardSlots;

    public SlotGroup SelectedGroup { get; set; }

    public void SendFairyCardsAndGameStart()
    {
        if (fairyCardSlots.slots[fairyCardSlots.slots.Count - 1].SelectedInvenItem == null)
            return;

        for (int i = 0; i < fairyCardSlots.slots.Count; i++)
        {
            GameManager.Instance.Team[i] = fairyCardSlots.slots[i].SelectedInvenItem as FairyCard;
        }
        GameManager.Instance.SceneLoad("03.BattleScene 1");
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
