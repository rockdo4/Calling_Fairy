using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemIcon itemIcon;
    public TextMeshProUGUI text;
    public event Action<ItemButton> OnClick;

    private int count = 0;

    private void Start()
    {
        SetButton();
    }

    public void UseItem()
    {
        if (count == 0)
            return;
        itemIcon.item.Count -= count;
        count = 0;
        SetButton();
    }

    public void SetButton()
    {
        itemIcon.SetIcon();
        text.text = $"{count}";
    }

    public void CountUp()
    {
        if (count >= itemIcon.item.Count)
            return;

        text.text = $"{++count}";

        if (OnClick != null)
            OnClick(this);
    }
}
