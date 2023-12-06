using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : SlotItem
{
    public Item Item
    {
        get
        {
            return (Item)inventoryItem;
        }
        private set
        {
            inventoryItem = value;
        }
    }

    public TextMeshProUGUI text;
    public Image image;

    public override void Init(InventoryItem invItem)
    {
        Item = invItem as Item;
        SetIcon();
    }

    public void SetIcon()
    {
        UpdateCount();
        //아이콘 이미지 셋
    }

    public void UpdateCount()
    {
        text.text = $"x{Item.Count}";
    }
}
