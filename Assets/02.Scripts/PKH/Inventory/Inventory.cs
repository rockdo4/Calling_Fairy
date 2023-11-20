using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory<T> : IInventory where T : IItem
{
    private List<T> items = new List<T>();

    public void AddItem(Progress.Item item)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveItem(Progress.Item item)
    {
        throw new System.NotImplementedException();
    }

}
