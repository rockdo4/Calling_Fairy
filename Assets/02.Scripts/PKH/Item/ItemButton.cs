using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : SlotItem
{
    public ItemIcon itemIcon;
    public TextMeshProUGUI text;
    public event Func<Item, bool> OnClick;
    public bool Limit { get; private set; } = false;

    private int count = 0;


    private void Start()
    {
        UpdateCount();
    }

    public override void Init(InventoryItem item)
    {
        itemIcon.Init(item);
        UpdateCount();
    }

    public void UpdateCount()
    {
        text.text = $"{count}";
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
        if (OnClick != null)
        {
            Limit = OnClick(itemIcon.Item);
        }

        if (count >= itemIcon.Item.Count || Limit)
            return;

        text.text = $"{++count}";
    }


}
