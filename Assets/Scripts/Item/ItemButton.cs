using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : InvGO
{
    public ItemIcon itemIcon;
    public TextMeshProUGUI text;
    public TextMeshProUGUI stockText;
    public Button minumButton;
    public Button plusButton;
    public event Action<ItemButton> OnAddButtonClick;
    public event Action<ItemButton> OnSubtractButtonClick;
    public event Func<bool> OnSimulation;

    public bool LimitLock { get; private set; } = false;

    public int Count { get; private set; } = 0;


    public override void Init(InventoryItem item)
    {
        Count = 0;
        itemIcon.Init(item);
        UpdateCount();
    }

    public void UpdateCount()
    {
        text.text = $"{Count}";
        itemIcon.UpdateCount();
    }

    public void UseItem()
    {
        if (Count <= 0 || Count > itemIcon.Item.Count)
            return;

        itemIcon.Item.Count -= Count;
        Count = 0;
        UpdateCount();
    }

    public void CountUp()
    {
        if (Count >= itemIcon.Item.Count || LimitLock)
            return;

        if (OnAddButtonClick != null)
            OnAddButtonClick(this);

        if (OnSimulation != null)
        {
            LimitLock = !OnSimulation();
            if (LimitLock)
                return;
            text.text = $"{++Count}";
        }
    }

    public void CountDown()
    {
        if (Count <= 0)
            return;

        if (OnSubtractButtonClick != null)
            OnAddButtonClick(this);

        if (OnSimulation != null)
        {
            OnSimulation();
            text.text = $"{--Count}";
        }
    }
}
