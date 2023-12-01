using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : SlotItem
{
    public Item item;

    public TextMeshProUGUI text;
    public Image image;

    public override void Init(InventoryItem invItem)
    {
        item = invItem as Item;
        text.text = 'x' + item.Count.ToString();
    }

    public void SetIcon()
    {
        UpdateCount();
        //아이콘 이미지 셋
    }

    public void UpdateCount()
    {
        text.text = $"x{item.Count}";
    }
}
