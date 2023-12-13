using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Slot : MonoBehaviour
{
    public int slotNumver;
    public SlotGroup slotGroup;
    public UnityEvent onSlotSelected;
    public UnityEvent onSlotDeselected;
    public InventoryItem SelectedInvenItem { get; private set; }

    protected Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public virtual void SetSlot(InventoryItem item)
    {
        SelectedInvenItem = item;
    }

    public virtual void UnsetSlot()
    {       
        SelectedInvenItem = null;
    }
}
