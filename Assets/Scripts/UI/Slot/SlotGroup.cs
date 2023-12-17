using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class SlotGroup<T> : SlotGroupBase where T : Slot
{
    public List<T> slots = new List<T>();
    public T SelectedSlot { get; set; }

    protected void Awake()
    {
        foreach (var slot in slots)
        {
            slot.SlotGroup = this;
        }
    }
}
