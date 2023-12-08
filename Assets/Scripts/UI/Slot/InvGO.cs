using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvGO : MonoBehaviour
{
    public InventoryItem inventoryItem;
    public abstract void Init(InventoryItem item);
}
