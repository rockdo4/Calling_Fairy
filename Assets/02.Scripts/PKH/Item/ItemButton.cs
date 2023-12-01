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

    private int count = 0;
    private bool Limit {  get; set; }

    private void Start()
    {
        SetButton();
    }

    public override void Init(InventoryItem item)
    {
        itemIcon.Init(item);
        SetButton();
    }

    public void SetButton()
    {
        itemIcon.SetIcon();
        text.text = $"{count}";
    }

    public void UseItem()
    {
        if (count == 0)
            return;
        itemIcon.item.Count -= count;
        count = 0;
        SetButton();
    }

    public void CountUp()
    {
        if (count >= itemIcon.item.Count || Limit)
            return;

        text.text = $"{++count}";

        if (OnClick != null)
        {
            Limit = OnClick(itemIcon.item);
        }
    }
}
