using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardButton : InvGO
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void Init(InventoryItem item)
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        transform.localScale = Vector3.one;
#endif
        inventoryItem = item;
        text.text = $"ID: {inventoryItem.ID}";
    }
}
