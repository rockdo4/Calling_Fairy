using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlotGroup : SlotGroup<EquipSlot>, IUIElement
{
    public void Init(Card card)
    {
        foreach (var slot in slots)
        {
            slot.Init(card);
        }
    }
}
