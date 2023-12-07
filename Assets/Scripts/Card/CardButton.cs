using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardButton : InvGO
{
    public bool IsUse { get; set; } = false;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void Init(InventoryItem item)
    {
        inventoryItem = item;
        text.text = $"ID: {inventoryItem.ID}";
    }
}
