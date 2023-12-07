using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotGroup : MonoBehaviour
{
    public List<Slot> slots = new List<Slot>();

    public UnityEvent onSlotSelected;
    public UnityEvent onSlotDeselected;
    public Slot SelectedSlot { get; set; }
 
}
