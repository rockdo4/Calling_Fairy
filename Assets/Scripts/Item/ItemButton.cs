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
    public event Func<Item, bool, bool> OnClick;
    public bool LimitLock { get; private set; } = false;

    private int count = 0;


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
        if (count <= 0 || count > itemIcon.Item.Count)
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
            LimitLock = !OnClick(itemIcon.Item, true);
            if (LimitLock)
                return;
            text.text = $"{++count}";
        }
    }

    public void CountDown()
    {
        if (count <= 0)
            return;

        if (OnClick != null)
        {
            LimitLock = !OnClick(itemIcon.Item, false);
            text.text = $"{--count}";
        }
    }
}
