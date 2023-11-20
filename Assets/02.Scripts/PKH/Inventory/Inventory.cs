using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<T> where T : Item
{
    private Dictionary<int, T> inventory = new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (inventory.TryGetValue(item.ID, out T value))
        {
            value.Count++;
        }
        else
        {
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
}
