using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotGroup : MonoBehaviour
{
    public enum Mode
    {
        SelectCard,
        SelectLeader,
    }

    public List<Slot> slots = new List<Slot>();

    public UnityEvent onSlotSelected;
    public UnityEvent onSlotDeselected;
    public Slot SelectedSlot { get; set; }
    public Mode CurrentMode { get; set; } = Mode.SelectCard;
}
