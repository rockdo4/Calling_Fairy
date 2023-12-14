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

    [Header("Select Card Mode")]
    [Tooltip("슬롯의 카드를 선택하려고 할 때 발생하는 이벤트")]
    public UnityEvent onSlotSelected;
    public UnityEvent onSlotDeselected;

    [Header("Select Leader Mode")]
    [Tooltip("리더를 선택하려고 할 때 발생하는 이벤트")]
    public UnityEvent onSlotDeselected2;

    public Slot SelectedSlot { get; set; }
    public Mode CurrentMode { get; set; } = Mode.SelectCard;
}
