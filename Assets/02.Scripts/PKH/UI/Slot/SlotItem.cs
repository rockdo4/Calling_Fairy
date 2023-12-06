using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotItem : MonoBehaviour
{
    public InventoryItem inventoryItem;
    public abstract void Init(InventoryItem item);
}
