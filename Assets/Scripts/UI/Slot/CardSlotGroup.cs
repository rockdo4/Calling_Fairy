using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSlotGroup : SlotGroup<CardSlot>
{
    public enum Mode
    {
        SelectCard,
        SelectLeader,
    }
    public Mode CurrentMode { get; set; } = Mode.SelectCard;
    public ToggleGroup ToggleGroup { get; private set; }

    [Header("Select Leader Mode")]
    [Tooltip("리더를 선택하려고 할 때 발생하는 이벤트")]
    public UnityEvent OnSelectLeader;
    public UnityEvent<int> OnSelectLeader2;

    private new void Awake()
    {
        base.Awake();
        ToggleGroup = GetComponent<ToggleGroup>();
    }
}
