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
    [Tooltip("������ ī�带 �����Ϸ��� �� �� �߻��ϴ� �̺�Ʈ")]
    public UnityEvent onSlotSelected;
    public UnityEvent onSlotDeselected;

    [Header("Select Leader Mode")]
    [Tooltip("������ �����Ϸ��� �� �� �߻��ϴ� �̺�Ʈ")]
    public UnityEvent onSlotDeselected2;

    public Slot SelectedSlot { get; set; }
    public Mode CurrentMode { get; set; } = Mode.SelectCard;
}
