using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : InventoryItem
{
    public int Count { get; set; } = 1;
}
