using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory<T> where T : Item
{

    private Dictionary<int, T> inventory = new Dictionary<int, T>();

    public Dictionary<int, T> Inven
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public void AddItem(T item)
    {
        if (inventory.TryGetValue(item.ID, out T value))
        {
            value.Count += item.Count;
        }
        else
        {
            inventory.Add(item.ID, item);
        }
    }

    public void AddItem(T item, int num)
    {
        if (inventory.TryGetValue(item.ID, out T value))
        {
            value.Count += num;
        }
        else
        {
            item.Count = num;
            inventory.Add(item.ID, item);
        }
    }

    public void RemoveItem(T item)
    {
        if (inventory.TryGetValue(item.ID, out T value) && value.Count != 0)
        {
            value.Count--;
        }
    }

    public void RemoveItem(T item, int num)
    {
        if (inventory.TryGetValue(item.ID, out T value) && value.Count >= num)
        {
            value.Count -= num;
        }
    }
}
