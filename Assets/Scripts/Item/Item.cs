using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : InventoryItem
{
    public int Count { get; set; } = 1;

    public Item (int id, int count = 1)
    {
        ID = id;
        Count = count;
    }

    public void UseItem()
    {
        Count--;
        Count = Count < 0 ? 0 : Count;
    }

    public void UseItem(int count)
    {
        Count -= count;
        Count = Count < 0 ? 0 : Count;
    }
}
