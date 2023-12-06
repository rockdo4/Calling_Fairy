using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : SlotItem
{
    public ItemIcon itemIcon;
    public TextMeshProUGUI text;
    public event Func<Item, bool> OnClick;
    public bool LimitLock { get; private set; } = false;

    private int count = 0;


    private void Start()
    {
        UpdateCount();
    }

    public override void Init(InventoryItem item)
    {
        count = 0;
        itemIcon.Init(item);
        UpdateCount();
    }

    public void UpdateCount()
    {
        text.text = $"{count}";
        itemIcon.UpdateCount();
    }

    public void UseItem()
    {
        if (count == 0)
            return;
        itemIcon.Item.Count -= count;
        count = 0;
        UpdateCount();
    }

    public void CountUp()
    {
        if (count >= itemIcon.Item.Count || LimitLock)
            return;

        if (OnClick != null)
        {
            LimitLock = !OnClick(itemIcon.Item);
        }

        text.text = $"{++count}";
    }
}
